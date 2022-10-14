using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public class LastName : ValueObject
{
    public const int MaxLenght = 50;
    public string Value { get; }
    private LastName(string value)
    {
        Value = value;
    }
    public static Result<LastName> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<LastName>(new Error("LastName.EmptyOrNull", "FirstName is empty."));
        }

        if (value.Length > MaxLenght)
        {
            return Result.Failure<LastName>(new Error("LastName.TooLong", "FirstName is too long."));
        }

        return new LastName(value);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
