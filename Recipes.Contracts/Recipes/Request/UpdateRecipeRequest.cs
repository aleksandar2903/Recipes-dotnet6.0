using Recipes.Contracts.Recipes.Request.Common;

namespace Recipes.Contracts.Recipes.Request;

public record UpdateRecipeRequest(
    string Title,
    string Description,
    Uri VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories,
    UpdateSectionsRequest Sections,
    UpdateInstructionsRequest Instructions) :
    RecipeRequest(
        Title,
        Description,
        VideoUrl,
        ThumbnailUrl,
        NumServings,
        TotalTimeMinutes,
        Calories);


