using FluentValidation;
using MediatR;
using Recipes.Domain.Shared;
using System.ComponentModel;

namespace Recipes.Application.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        Error[] errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(
                failure.ErrorCode,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if(typeof(TResult) == typeof(Result))
        {
            return (ValidateResult.WithErrors(errors) as TResult)!;
        }

        object valiadtionResult = typeof(ValidateResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidateResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)valiadtionResult;
    }
}
