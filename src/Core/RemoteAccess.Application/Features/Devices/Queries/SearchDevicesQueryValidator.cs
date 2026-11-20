using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Devices.Queries;

public sealed class SearchDevicesQueryValidator : AbstractValidator<SearchDevicesQuery>
{
    public SearchDevicesQueryValidator()
    {
        RuleFor(x => x.Search).NotEmpty().WithMessage(ValidationErrorMessages.DeviceSearchRequired);
    }
}
