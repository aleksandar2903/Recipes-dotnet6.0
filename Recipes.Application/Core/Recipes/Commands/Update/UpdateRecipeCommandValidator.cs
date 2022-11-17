using FluentValidation;
using Recipes.Application.Core.Recipes.Commands.Common.Validation;

namespace Recipes.Application.Core.Recipes.Commands.Update;

public sealed class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
{
	public UpdateRecipeCommandValidator()
	{
		RuleFor(recipe => recipe).SetValidator(new RecipeValidator());
	}
}
