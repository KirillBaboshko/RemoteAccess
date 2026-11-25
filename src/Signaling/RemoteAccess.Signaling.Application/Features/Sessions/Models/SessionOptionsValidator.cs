using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public sealed class SessionOptionsValidator : AbstractValidator<SessionOptions>
{
    public SessionOptionsValidator()
    {
        RuleFor(x => x.PendingTimeout).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.PendingTimeoutInvalid);
        RuleFor(x => x.NegotiationTimeout).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.NegotiationTimeoutInvalid);
        RuleFor(x => x.ConnectionTimeout).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.ConnectionTimeoutInvalid);
        RuleFor(x => x.RecoveryTimeout).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.RecoveryTimeoutInvalid);
        RuleFor(x => x.CleanupInterval).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
        RuleFor(x => x.ConnectionLease).GreaterThan(x => x.CleanupInterval * 2).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
        RuleFor(x => x.SessionLease).GreaterThan(x => x.CleanupInterval * 2).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
        RuleFor(x => x.CleanupGrace).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
    }
}
