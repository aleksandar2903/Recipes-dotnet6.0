using Recipes.Domain.Primitives;

namespace Recipes.Domain.Entities;

public sealed class Ingredient : Entity, IAuditableEntity
{
    private Ingredient() { }
    private Ingredient(Guid id, Guid sectionId, int position, string text) : base(id)
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

    public static Ingredient Create(Guid id, Guid sectionId, int position, string text)
    {
        return new Ingredient(id, sectionId, position, text);
    }
    public void UpdateText(string text)
    {
        Text = text;
    }
}
