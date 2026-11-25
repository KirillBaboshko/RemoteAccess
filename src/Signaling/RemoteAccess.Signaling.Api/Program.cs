using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using RemoteAccess.Common.Redis;
using RemoteAccess.Signaling.Api.Hubs;
using RemoteAccess.Signaling.Application;
using SessionOptions = RemoteAccess.Signaling.Application.Features.Sessions.Models.SessionOptions;
using ValidationErrorMessages = RemoteAccess.Signaling.Domain.Exceptions.ValidationErrorMessages;
using RemoteAccess.Signaling.Infrastructure;
using RemoteAccess.Signaling.Infrastructure.Settings;
using RemoteAccess.Signaling.Application.Features.WebRtc.Models;
using FluentValidation;
using RemoteAccess.Contracts.Signaling.Devices;
using RemoteAccess.Signaling.Api.Grpc.Devices;
using RemoteAccess.Signaling.Api.Grpc.Interceptors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc(options => options.Interceptors.Add<GrpcExceptionInterceptor>());
builder.Services.AddScoped<IValidator<GetDeviceAvailabilityRequest>, GetDeviceAvailabilityRequestValidator>();
builder.Services.AddScoped<IValidator<DeviceStatusChangedRequest>, DeviceStatusChangedRequestValidator>();

builder.Services.AddSignalR(options =>
    {
        options.AddFilter<SignalingExceptionFilter>();
        options.MaximumReceiveMessageSize = WebRtcMessageLimits.HubMessageBytes;
    })
    .AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSignalingNotifier<SignalingHub>();

var secret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(secret))
    throw new InvalidOperationException(ValidationErrorMessages.JwtSecretRequired);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{" + secret + "}")),
        ClockSkew = TimeSpan.FromMinutes(1)
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Path.StartsWithSegments("/hubs/signaling") &&
                context.Request.Query.TryGetValue("access_token", out var token))
                context.Token = token;
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
    if (origins.Length > 0)
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
}));
builder.Services.AddApplication();
builder.Services.AddAndConfigureInfrastructure(new InfrastructureSettings
{
    RedisSettings = builder.Configuration.GetSection("Redis").Get<RedisClientSettings>() ?? new(),
    SessionSettings = builder.Configuration.GetSection("Sessions").Get<SessionOptions>() ?? new(),
    CoreSettings = builder.Configuration.GetSection("Core").Get<CoreApiSettings>() ?? new()
});

var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<SignalingHub>("/hubs/signaling", options => options.CloseOnAuthenticationExpiration = true);
app.MapGrpcService<DeviceAvailabilityGrpcService>().RequireAuthorization(policy =>
    policy.RequireAssertion(context => Guid.TryParse(context.User.FindFirst("userId")?.Value, out var userId)
        && userId != Guid.Empty));
app.Run();
