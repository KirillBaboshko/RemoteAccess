using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class SessionHistoryQueryValidator : AbstractValidator<SessionHistoryQuery>
{
    public SessionHistoryQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.UserRequired);
        RuleFor(x => x.Filter).NotNull().WithMessage(ValidationErrorMessages.HistoryFilterRequired)
            .SetValidator(new SessionHistoryFilterValidator());
    }
}
