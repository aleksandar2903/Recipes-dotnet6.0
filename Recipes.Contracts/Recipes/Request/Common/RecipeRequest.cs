namespace Recipes.Contracts.Recipes.Request.Common;

public record RecipeRequest(
    string Title,
    string Description,
    Uri VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories);
public record SectionRequest(string Text);
public record InstructionRequest(string Text);
public record IngredientRequest(string Text);