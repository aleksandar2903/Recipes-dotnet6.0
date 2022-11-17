using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Core.Recipes.Commands.Common.Request;

namespace Recipes.Application.Core.Recipes.Commands.Update;

public sealed record UpdateRecipeCommand(
    Guid? RecipeId,
    string Title,
    string Description,
    Uri? VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories) : Recipe(
        Title,
        Description,
        VideoUrl,
        ThumbnailUrl,
        NumServings,
        TotalTimeMinutes,
        Calories), ICommand;
