using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Section : ValueObject
{
    public const int MaxLenghtOfText = 150;
    public string Text { get; }
    public Guid Id { get; }
    public IReadOnlyCollection<Ingredient> Ingredients { get; }
    private Section(Guid id, string text, List<Ingredient> ingredients)
    {
        Id = id;
        Text = text;
        Ingredients = ingredients;
    }
    public static Result<Section> Create(Guid? id, string text, List<Ingredient> ingredients)
    {
        return new Section(id ?? Guid.Empty, text, ingredients);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Text;
    }
}
