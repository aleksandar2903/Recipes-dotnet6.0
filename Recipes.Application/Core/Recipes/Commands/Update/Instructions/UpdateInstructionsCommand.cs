using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Core.Recipes.Commands.Add;

namespace Recipes.Application.Core.Recipes.Commands.Update.Instructions;

public record UpdateInstructionsCommand(
    string? UserId,
    Guid RecipeId,
    List<InstructionRequest> Instructions) : ICommand;
