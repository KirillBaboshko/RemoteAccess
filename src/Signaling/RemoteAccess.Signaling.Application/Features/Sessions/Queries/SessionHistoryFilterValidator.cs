using FluentValidation;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class SessionHistoryFilterValidator : AbstractValidator<SessionHistoryFilter>
{
    public SessionHistoryFilterValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage(ValidationErrorMessages.HistoryPageInvalid)
            .Must((filter, page) => ((long)page - 1) * filter.PageSize <= int.MaxValue)
            .WithMessage(ValidationErrorMessages.HistoryPageInvalid);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage(ValidationErrorMessages.HistoryPageSizeInvalid);
        RuleFor(x => x.DeviceId).NotEqual(Guid.Empty).When(x => x.DeviceId.HasValue)
            .WithMessage(ValidationErrorMessages.DeviceRequired);
        RuleFor(x => x.State).Must(state => state is SessionState.Ended or SessionState.Rejected or SessionState.Expired)
            .When(x => x.State.HasValue).WithMessage(ValidationErrorMessages.HistoryStateInvalid);
        RuleFor(x => x.To).Must((filter, to) => to > filter.From)
            .When(x => x.From.HasValue && x.To.HasValue).WithMessage(ValidationErrorMessages.HistoryPeriodInvalid);
    }
}
