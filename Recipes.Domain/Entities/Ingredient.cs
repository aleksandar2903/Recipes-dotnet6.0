using Recipes.Domain.Primitives;

namespace Recipes.Domain.Entities;

public sealed class Ingredient : Entity, IAuditableEntity
{
    private Ingredient() { }
    internal Ingredient(Guid id, Guid sectionId, int position, string text) : base(id)
    {
        SectionId = sectionId;
        Position = position;
        Text = text;
    }

    public string Text { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public Guid SectionId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    internal void UpdateInformations(int position, string text)
    {
        Position = position;
        Text = text;
    }
}
