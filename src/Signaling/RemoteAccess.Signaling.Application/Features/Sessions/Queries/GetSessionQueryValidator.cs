using FluentValidation;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class GetSessionQueryValidator : AbstractValidator<GetSessionQuery>
{
    public GetSessionQueryValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.SessionId).NotEmpty().WithMessage(ValidationErrorMessages.SessionRequired);
    }
}
