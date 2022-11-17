using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Core.Recipes.Commands.Common.Request;

namespace Recipes.Application.Core.Recipes.Commands.Update.Instructions;

public record UpdateInstructionsCommand(
    Guid RecipeId,
    IReadOnlyCollection<UpdateInstruction> Instructions) : ICommand;
public record UpdateInstruction(Guid? Id, string Text) : Instruction(Text);