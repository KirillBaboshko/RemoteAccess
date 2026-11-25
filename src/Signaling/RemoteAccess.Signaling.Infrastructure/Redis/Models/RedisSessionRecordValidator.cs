using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Infrastructure.Redis.Models;

public sealed class RedisSessionRecordValidator : AbstractValidator<RedisSessionRecord>
{
    public static RedisSessionRecordValidator Default { get; } = new();

    public RedisSessionRecordValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.Version).GreaterThan(0).WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.State).IsInEnum().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.NegotiationState).IsInEnum().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.SourceConnectionState).IsInEnum().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.TargetConnectionState).IsInEnum().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.EndReason).IsInEnum().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        RuleFor(x => x.Source).NotNull().WithMessage(ValidationErrorMessages.RedisSessionInvalid).ChildRules(participant =>
        {
            participant.RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
            participant.RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
            participant.RuleFor(x => x.ConnectionId).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        });
        RuleFor(x => x.Target).NotNull().WithMessage(ValidationErrorMessages.RedisSessionInvalid).ChildRules(participant =>
        {
            participant.RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
            participant.RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
            participant.RuleFor(x => x.ConnectionId).NotEmpty().WithMessage(ValidationErrorMessages.RedisSessionInvalid);
        });
        RuleForEach(x => new long?[] { x.CreatedAt, x.ExpiresAt, x.AcceptedAt, x.EndedAt, x.NegotiationExpiresAt,
                x.ConnectionExpiresAt, x.RecoveryExpiresAt, x.ConnectedAt, x.UpdatedAt })
            .Must(value => !value.HasValue || value >= DateTimeOffset.MinValue.ToUnixTimeMilliseconds()
                && value <= DateTimeOffset.MaxValue.ToUnixTimeMilliseconds())
            .WithMessage(ValidationErrorMessages.RedisSessionInvalid).OverridePropertyName("Timestamps");
    }
}
