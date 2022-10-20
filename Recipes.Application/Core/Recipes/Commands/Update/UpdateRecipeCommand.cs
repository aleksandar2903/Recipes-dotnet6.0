using Recipes.Application.Abstractions.Messaging;

namespace Recipes.Application.Core.Recipes.Commands.Update;

public sealed record UpdateRecipeCommand(
    string? UserId,
    Guid? RecipeId,
    string Title,
    string Description,
    Uri? VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories) : ICommand;
