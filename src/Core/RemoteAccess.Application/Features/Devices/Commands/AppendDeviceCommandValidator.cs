using FluentValidation;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Domain.Models.Ecxeptions;
using RemoteAccess.Domain.Repositories;
namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class AppendDeviceCommandValidator : AbstractValidator<AppendDeviceCommand>
    {
        public AppendDeviceCommandValidator(IUsersRepository usersRepository)
        {
            RuleFor(x => x.DeviceInfo)
                .NotNull()
                .SetValidator(DeviceInfoValidator.Default);

            RuleFor(x => x.AuthenticatedUserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MustAsync(async (id, cancellationToken) =>
                    await usersRepository.GetByIdAsync(id, cancellationToken) is not null)
                .WithMessage(ValidationErrorMessages.DeviceOwnerNotFound);

            RuleFor(x => x.DeviceInfo.OwnerId)
                .Equal(x => x.AuthenticatedUserId)
                .When(x => x.DeviceInfo is not null)
                .WithMessage(ValidationErrorMessages.DeviceOwnerMismatch);
        }
    }
}
