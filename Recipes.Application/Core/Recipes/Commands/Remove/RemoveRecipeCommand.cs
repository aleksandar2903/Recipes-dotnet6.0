using Recipes.Application.Abstractions.Messaging;

namespace Recipes.Application.Core.Recipes.Commands.Remove;

public sealed record RemoveRecipeCommand(Guid RecipeId) : ICommand;
