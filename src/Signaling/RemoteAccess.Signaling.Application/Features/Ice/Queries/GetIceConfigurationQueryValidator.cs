using FluentValidation;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Ice.Queries;

public sealed class GetIceConfigurationQueryValidator : AbstractValidator<GetIceConfigurationQuery>
{
    public GetIceConfigurationQueryValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
    }
}
