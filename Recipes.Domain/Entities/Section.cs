using Recipes.Domain.Primitives;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities;

public sealed class Section : Entity, IAuditableEntity
{
    private Section() { }
    internal Section(Guid id, Guid recipeId, int position, string text, List<Ingredient> ingredients) : base(id)
    {
        RecipeId = recipeId;
        Position = position;
        Text = text;
        _ingredients = ingredients;
    }
    private List<Ingredient> _ingredients = new();
    public string Text { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public Guid RecipeId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;
    public Ingredient AddIngredient(Guid id, int position, string text)
    {
        var ingredient = new Ingredient(id, Id, position, text);
        _ingredients.Add(ingredient);
        return ingredient;
    }
    internal void RemoveIngredients(IReadOnlyCollection<ValueObjects.Ingredient> ingredients)
    {
        _ingredients.RemoveAll(ingredient => !ingredients.Any(i => i.Id == ingredient.Id));
    }

    internal void UpdateInformations(int position, string text)
    {
        Position = position;
        Text = text;
    }
}
