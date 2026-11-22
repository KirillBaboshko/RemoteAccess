using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public sealed class SessionOptionsValidator : AbstractValidator<SessionOptions>
{
    public SessionOptionsValidator()
    {
        RuleFor(x => x.PendingTimeout).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.PendingTimeoutInvalid);
        RuleFor(x => x.MaintenanceInterval).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
        RuleFor(x => x.ConnectionLease).GreaterThan(x => x.MaintenanceInterval * 2).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
        RuleFor(x => x.SessionLease).GreaterThan(x => x.MaintenanceInterval * 2).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
        RuleFor(x => x.CleanupGrace).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.SessionLeaseOptionsInvalid);
    }
}
