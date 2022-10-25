using Recipes.Domain.Primitives;

namespace Recipes.Domain.Entities;

public class Instruction : Entity, IAuditableEntity
{
    private Instruction() { }
    internal Instruction(Guid id, Guid recipeId, int position, string text) : base(id)
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

    internal void UpdateInformations(int position, string text)
    {
        Position = position;
        Text = text;
    }
}
