using FluentValidation;
using MediatR;
using Skills.Domain.Common.Exceptions;

namespace Skills.Application.Validation;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators = validators;

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var errors = validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .Select(x => x.ErrorMessage)
            .Distinct()
            .ToArray();

        if (errors.Length != 0) throw new AppException(
            ExceptionCode.BadRequest,
            ExceptionMessages.BadRequest.Format,
            string.Join("\n", errors)
        );

        return await next();
    }
}