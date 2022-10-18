using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public class Position : ValueObject
{
    public const int MaxLength = 100;
    public const int MinLength = 0;
    public int Value { get; }
    private Position(int value)
    {
        Value = value;
    }
    public static Result<Position> Create(int value)
    {
        return new Position(value);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
