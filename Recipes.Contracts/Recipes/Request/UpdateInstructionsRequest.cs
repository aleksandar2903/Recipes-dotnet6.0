using Recipes.Contracts.Recipes.Request.Common;

namespace Recipes.Contracts.Recipes.Request;

public record UpdateInstructionsRequest(IReadOnlyCollection<UpdateInstructionRequest> Instructions);
public record UpdateInstructionRequest(
    Guid? Id,
    string Text) : InstructionRequest(Text);

