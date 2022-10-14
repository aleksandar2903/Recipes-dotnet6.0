using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public const int MaxLenght = 256;
    public string Value { get; }
    private Email(string value)
    {
        Value = value;
    }
    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<Email>(new Error("Email.EmptyOrNull", "Email is empty."));
        }

        if (value.Length > MaxLenght)
        {
            return Result.Failure<Email>(new Error("Email.TooLong", "Email is too long."));
        }

        return new Email(value);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}