namespace Recipes.Domain.Shared;

public class ValidateResult<T> : Result<T>, IValidateResult
{
    private ValidateResult(Error[] errors) : base(default, false, IValidateResult.ValidateError)
        => Errors = errors;

    public Error[] Errors { get; }

    public static ValidateResult<T> WithErrors(Error[] errors) => new(errors);
}
