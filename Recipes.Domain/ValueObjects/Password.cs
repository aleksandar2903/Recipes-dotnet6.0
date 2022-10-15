using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Password : ValueObject
{
    public const int MaxLenght = 256;
    public const int MinLenght = 64;
    public string Value { get; }
    private Password(string value)
    {
        Value = value;
    }
    public static Result<Password> Create(string value)
    {
        return new Password(value);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
