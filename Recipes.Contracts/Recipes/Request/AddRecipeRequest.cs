using Recipes.Contracts.Recipes.Request.Common;

namespace Recipes.Contracts.Recipes.Request;

public record AddRecipeRequest(
    string Title,
    string Description,
    Uri VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories,
    IReadOnlyCollection<AddSectionRequest> Sections,
    IReadOnlyCollection<AddInstructionRequest> Instructions) : 
    RecipeRequest(
        Title,
        Description, 
        VideoUrl, 
        ThumbnailUrl, 
        NumServings, 
        TotalTimeMinutes,
        Calories);
public record AddIngredientRequest(
    string Text) : IngredientRequest(Text);
public record AddSectionRequest(
    string Text,
    IReadOnlyCollection<UpdateIngredientRequest> Ingredients) : SectionRequest(Text);
public record AddInstructionRequest(
    string Text) : InstructionRequest(Text);

