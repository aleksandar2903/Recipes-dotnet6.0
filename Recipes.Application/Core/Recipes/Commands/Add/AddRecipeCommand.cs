using Recipes.Application.Abstractions.Messaging;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public record AddRecipeCommand(
    string? UserId,
    string Title,
    string Description,
    Uri? VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories,
    IReadOnlyCollection<SectionRequest> Sections,
    IReadOnlyCollection<InstructionRequest> Instructions) : ICommand<Guid>;
public record SectionRequest(string Text, List<IngredientRequest> Ingredients);
public record InstructionRequest(string Text);
public record IngredientRequest(string Text);
