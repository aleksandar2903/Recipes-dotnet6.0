using Recipes.Domain.Primitives;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities;

public sealed class Instruction : Entity, IAuditableEntity
{
    private Instruction(Guid id, Guid recipeId, int position, string text) : base(id)
    {
        RecipeId = recipeId;
        Position = position;
        Text = text;
    }

    public string Text { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public Guid RecipeId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }

    public static Instruction Create(Guid id, Guid recipeId, int position, string text)
    {
        return new Instruction(id, recipeId, position, text);
    }
}
