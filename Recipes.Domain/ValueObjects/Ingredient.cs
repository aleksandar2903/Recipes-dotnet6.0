using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Ingredient : ValueObject
{
    public const int MaxLenghtOfText = 150;
    public string Text { get; }
    public Guid Id { get; }
    private Ingredient(Guid id, string text)
    {
        Id = id;
        Text = text;
    }
    public static Result<Ingredient> Create(Guid? id, string text)
    {
        return new Ingredient(id ?? Guid.Empty, text);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Text;
    }
}
