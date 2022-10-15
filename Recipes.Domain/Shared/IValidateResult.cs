namespace Recipes.Domain.Shared;

public interface IValidateResult
{
    public static readonly Error ValidateError = new("ValidateError", "A validate problem occured.");
    Error[] Errors { get; }
}
