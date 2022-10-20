using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;
using System.Security.AccessControl;

namespace Recipes.Domain.Entities;

public sealed class Recipe : Entity, IAuditableEntity
{
    private Recipe() { }
    private Recipe(
        Guid id,
        Guid authorId,
        string title,
        string description,
        Uri? videoUrl,
        Uri thumbnailUrl,
        int? numServings,
        int totalTimeMinutes,
        int? calories,
        List<Section> sections,
        List<Instruction> instructions) : base(id)
    {
        AuthorId = authorId;
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        ThumbnailUrl = thumbnailUrl;
        NumServings = numServings;
        TotalTimeMinutes = totalTimeMinutes;
        Calories = calories;
        _sections = sections;
        _instructions = instructions;
    }
    private readonly List<Section> _sections = new();
    private readonly List<Instruction> _instructions = new();

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Uri? VideoUrl { get; private set; }
    public Uri ThumbnailUrl { get; private set; }
    public int? NumServings { get; private set; }
    public Guid AuthorId { get; private set; }
    public int TotalTimeMinutes { get; set; }
    public int? Calories { get; set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public IReadOnlyCollection<Section> Sections => _sections;
    public IReadOnlyCollection<Instruction> Instructions => _instructions;
    public User Author { get; private set; }

    public static Recipe Create(
        Guid id,
        Guid authorId,
        string title,
        string description,
        Uri? videoUrl,
        Uri thumbnailUrl,
        int? numServings,
        int totalTimeMinutes,
        int? calories,
        List<Section> sections,
        List<Instruction> instructions)
    {
        return new Recipe(
            id,
            authorId,
            title,
            description,
            videoUrl,
            thumbnailUrl,
            numServings,
            totalTimeMinutes,
            calories,
            sections,
            instructions);
    }
    public void UpdateInformations(
       string title,
       string description,
       Uri? videoUrl,
       Uri thumbnailUrl,
       int? numServings,
       int totalTimeMinutes,
       int? calories)
    {
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        ThumbnailUrl = thumbnailUrl;
        NumServings = numServings;
        TotalTimeMinutes = totalTimeMinutes;
        Calories = calories;
    }

    public Instruction AddInstruction(Guid id, int position, string text)
    {
        var instruction = Instruction.Create(id, Id, position, text);

        _instructions.Add(instruction);

        return instruction;
    }
    public void RemoveInstruction(Instruction instruction)
    {
        _instructions.Remove(instruction);
    }
    public void RemoveInstruction(Section section)
    {
        _sections.Remove(section);
    }
    public Section AddSection(Guid id, int position, string text, List<Ingredient> ingredients)
    {
        var section = Section.Create(id, Id, position, text, ingredients);

        _sections.Add(section);

        return section;
    }

    public void RemoveSection(Section section)
    {
        _sections.Remove(section);
    }
}
