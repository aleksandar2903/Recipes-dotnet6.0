using FluentValidation;
using Recipes.Application.Core.Recipes.Commands.Add;

namespace Recipes.Application.Core.Recipes.Commands.Update.Instructions;

public sealed class UpdateInstructionsCommandValidator : AbstractValidator<UpdateInstructionsCommand>
{
	public UpdateInstructionsCommandValidator()
	{
		RuleForEach(instructions => instructions.Instructions).SetValidator(new AddInstructionValidation());
	}
}
