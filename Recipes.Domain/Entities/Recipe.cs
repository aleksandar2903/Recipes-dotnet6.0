using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

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
        User author,
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
            author.Id,
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
    public void RemoveSections()
    {
        _sections.Clear();
    }
    public void AddSections(List<Section> sections)
    {
        _sections.AddRange(sections);
    }
    public void AddInstructions(List<Instruction> instructions)
    {
        _instructions.AddRange(instructions);
    }
}
