using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class SessionHistoryByIdQueryValidator : AbstractValidator<SessionHistoryByIdQuery>
{
    public SessionHistoryByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.UserRequired);
        RuleFor(x => x.SessionId).NotEmpty().WithMessage(ValidationErrorMessages.SessionRequired);
    }
}
