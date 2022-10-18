using Recipes.Domain.Primitives;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities;

public sealed class Section : Entity, IAuditableEntity
{
    private Section() { }
    private Section(Guid id, Guid recipeId, int position, string text, List<Ingredient> ingredients) : base(id)
    {
        RecipeId = recipeId;
        Position = position;
        Text = text;
        _ingredients = ingredients;
    }
    private readonly List<Ingredient> _ingredients = new();
    public string Text { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public Guid RecipeId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;

    public static Section Create(Guid id, Guid recipeId, int position, string text, List<Ingredient> ingredients)
    {
        return new Section(id, recipeId, position, text, ingredients);
    }
    public void AddIngredients(List<Ingredient> ingredients)
    {
        _ingredients.AddRange(ingredients);
    }

    public void UpdateText(string text)
    {
        Text = text;
    }
}
