using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Text : ValueObject
{
    public const int MaxLength = 100;
    public const int MinLength = 0;
    public string Value { get; }
    private Text(string value)
    {
        Value = value;
    }
    public static Result<Text> Create(string value)
    {
        return new Text(value);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
