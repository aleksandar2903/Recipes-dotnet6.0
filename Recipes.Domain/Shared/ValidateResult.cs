namespace Recipes.Domain.Shared;

public class ValidateResult : Result, IValidateResult
{
    private ValidateResult(Error[] errors) : base(false, IValidateResult.ValidateError)
        => Errors = errors;

    public Error[] Errors { get; }

    public static ValidateResult WithErrors(Error[] errors) => new(errors);
}
