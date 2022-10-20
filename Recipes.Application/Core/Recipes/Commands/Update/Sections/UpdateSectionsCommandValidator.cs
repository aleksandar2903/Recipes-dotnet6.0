using FluentValidation;
using Recipes.Application.Core.Recipes.Commands.Add;

namespace Recipes.Application.Core.Recipes.Commands.Update.Sections;

public sealed class UpdateSectionsCommandValidator : AbstractValidator<UpdateSectionsCommand>
{
	public UpdateSectionsCommandValidator()
	{
		RuleForEach(sections => sections.Sections).SetValidator(new AddSectionValidation());
	}
}
