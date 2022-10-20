namespace Recipes.Application.Contracts.Queries.Recipes;

public record RecipeResponse(Guid Id,
string Title,
Uri VideoUrl,
Uri ThumbnailUrl,
int? NumServings,
int TotalTimeMinutes,
int? Calories,
DateTime CreateOnUtc);
