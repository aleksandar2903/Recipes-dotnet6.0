using Recipes.Application.Abstractions.Messaging;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public record AddRecipeCommand(
    string Title,
    string Description,
    Uri? VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories,
    IReadOnlyCollection<SectionRequest> Sections,
    IReadOnlyCollection<InstructionRequest> Instructions) : ICommand<Guid>;
public record SectionRequest(Guid? Id, string Text, List<IngredientRequest> Ingredients);
public record InstructionRequest(Guid? Id, string Text);
public record IngredientRequest(Guid? Id, string Text);
