using FluentValidation;
using MediatR;

namespace RemoteAccess.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var errors = new List<FluentValidation.Results.ValidationFailure>();
        foreach (var validator in _validators)
            errors.AddRange((await validator.ValidateAsync(context, cancellationToken)).Errors);

        if (errors.Count != 0)
            throw new ValidationException(errors);

        return await next(cancellationToken);
    }
}
