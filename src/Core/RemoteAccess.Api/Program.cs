using System.Text;
using RemoteAccess.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using RemoteAccess.Application;
using RemoteAccess.Infrastructure;
using FluentValidation;
using RemoteAccess.Api.Grpc.Devices;
using RemoteAccess.Api.Grpc.Interceptors;
using RemoteAccess.Contracts.Core.Devices;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Data.DataProviders.Sessions;
using RemoteAccess.Domain.Models.Ecxeptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    var messages = options.ModelBindingMessageProvider;
    messages.SetAttemptedValueIsInvalidAccessor((_, _) => ValidationErrorMessages.RequestInvalid);
    messages.SetValueIsInvalidAccessor(_ => ValidationErrorMessages.RequestInvalid);
    messages.SetValueMustNotBeNullAccessor(_ => ValidationErrorMessages.RequestInvalid);
});
builder.Services.AddGrpc(options => options.Interceptors.Add<GrpcExceptionInterceptor>());
builder.Services.AddScoped<IValidator<GetDeviceAccessRequest>, GetDeviceAccessRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret не задан");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{" + jwtSecret + "}")),
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error switch
        {
            DeviceAvailabilityUnavailableException or SessionHistoryUnavailableException => StatusCodes.Status503ServiceUnavailable,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException or ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(new { error = error?.Message ?? "Неизвестная ошибка" });
    });
});

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), api => api.UseHttpsRedirection());
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<DeviceAccessGrpcService>().RequireAuthorization(policy =>
    policy.RequireAssertion(context => context.User.TryGetNonEmptyUserId(out _)));

app.Run();
