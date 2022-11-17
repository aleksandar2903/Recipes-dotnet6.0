using FluentValidation;
using Recipes.Application.Core.Recipes.Commands.Common.Validation;

namespace Recipes.Application.Core.Recipes.Commands.Update.Sections;

public sealed class UpdateSectionsCommandValidator : AbstractValidator<UpdateSectionsCommand>
{
	public UpdateSectionsCommandValidator()
	{
        RuleForEach(sections => sections.Sections).SetValidator(new UpdateSectionValidator());
    }
}
public sealed class UpdateSectionValidator : AbstractValidator<UpdateSection>
{
    public UpdateSectionValidator()
    {
        RuleFor(section => section).SetValidator(new SectionValidator());
        RuleFor(section => section.Ingredients).NotEmpty();
        RuleForEach(section => section.Ingredients).SetValidator(new IngredientValidator());
    }
}
