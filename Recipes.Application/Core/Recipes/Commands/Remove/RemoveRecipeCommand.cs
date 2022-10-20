using Recipes.Application.Abstractions.Messaging;

namespace Recipes.Application.Core.Recipes.Commands.Remove;

public sealed record RemoveRecipeCommand(string? UserId, Guid RecipeId) : ICommand;
