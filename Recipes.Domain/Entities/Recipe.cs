using Recipes.Domain.Primitives;

namespace Recipes.Domain.Entities;

public sealed class Recipe : AggregateRoot, IAuditableEntity
{
    #region Constructors
    private Recipe(Guid id) : base(id) { }
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
    #endregion

    #region Properties
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
    #endregion

    #region Methods
    /// <summary>
    /// Create new instance of recipe
    /// </summary>
    /// <param name="id">Identifier parameter.</param>
    /// <param name="authorId"> Author identifier parameter.</param>
    /// <param name="title">Title of the recipe.</param>
    /// <param name="description">Description of the recipe.</param>
    /// <param name="videoUrl"></param>
    /// <param name="thumbnailUrl"></param>
    /// <param name="numServings"></param>
    /// <param name="totalTimeMinutes"></param>
    /// <param name="calories"></param>
    /// <param name="sections"></param>
    /// <param name="instructions"></param>
    /// <returns></returns>
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
    List<ValueObjects.Section> sections,
    List<ValueObjects.Instruction> instructions)
    {
        int sectionPosition = 0;
        int instructionPosition = 0;
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
            sections.Select(section =>
            {
                int ingredientPosition = 0;
                return new Section(
                    Guid.Empty,
                    Guid.Empty,
                    ++sectionPosition,
                    section.Text,
                    section.Ingredients.Select(ingredient => new Ingredient(
                        Guid.Empty,
                        Guid.Empty,
                        ++ingredientPosition,
                        ingredient.Text)).ToList());
            }).ToList(),
            instructions.Select(instruction =>
                new Instruction(
                    Guid.Empty,
                    Guid.Empty,
                    ++instructionPosition,
                    instruction.Text)).ToList());
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

    public void UpdateInstructions(List<ValueObjects.Instruction> instructions)
    {
        //Delete exist instructions
        _instructions.RemoveAll(instruction => !instructions.Any(i => i.Id == instruction.Id));
        //Create new or update exist instructions
        int position = 0;
        foreach (var instruction in instructions)
        {
            Instruction existsInstruction = Instructions.FirstOrDefault(i => i.Id == instruction.Id);
            if (existsInstruction is not null)
            {
                existsInstruction.UpdateInformations(++position, instruction.Text);
            }
            else
            {
                _instructions.Add(new(Guid.Empty, Id, ++position, instruction.Text));
            }
        }
    }

    public void UpdateSections(List<ValueObjects.Section> sections)
    {
        //Delete exist sections with ingredients
        _sections.RemoveAll(i => !sections.Any(section => section.Id == i.Id));
        //Create new or update exist section with ingredients
        int sectionPosition = 0;
        foreach (var section in sections)
        {
            Section existsSection = _sections.FirstOrDefault(i => i.Id == section.Id);
            if (existsSection is not null)
            {
                existsSection.UpdateInformations(++sectionPosition, section.Text);
                existsSection.RemoveIngredients(section.Ingredients);

                int ingredientPosition = 0;
                foreach (var ingredient in section.Ingredients)
                {
                    Ingredient existsIngredient = existsSection.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
                    if (existsIngredient is not null)
                    {
                        existsIngredient.UpdateInformations(++ingredientPosition, ingredient.Text);
                    }
                    else
                    {
                        existsSection.AddIngredient(Guid.Empty, ++ingredientPosition, ingredient.Text);
                    }
                }
            }
            else
            {
                int ingredientPosition = 0;
                _sections.Add(new Section(
                    Guid.Empty,
                    Guid.Empty,
                    ++sectionPosition,
                    section.Text,
                    section.Ingredients.Select(ingredient =>
                        new Ingredient(
                                Guid.Empty,
                                Guid.Empty,
                                ++ingredientPosition,
                                ingredient.Text)
                        ).ToList()));
            }
        }
    }
    #endregion
}
