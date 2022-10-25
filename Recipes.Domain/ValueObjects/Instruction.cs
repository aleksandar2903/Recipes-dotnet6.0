using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Instruction : ValueObject
{
    public const int MaxLenghtOfText = 500;
    public string Text { get; }
    public Guid Id { get; }
    private Instruction(Guid id, string text)
    {
        Id = id;
        Text = text;
    }
    public static Result<Instruction> Create(Guid? id, string text)
    {
        return new Instruction(id ?? Guid.Empty, text);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Text;
    }
}
