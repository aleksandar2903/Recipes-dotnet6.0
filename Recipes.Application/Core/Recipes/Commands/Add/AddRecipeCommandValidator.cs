using FluentValidation;
using Recipes.Application.Core.Recipes.Commands.Common.Validation;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public class AddRecipeCommandValidator : AbstractValidator<AddRecipeCommand>
{
    public AddRecipeCommandValidator()
    {
        RuleFor(recipe => recipe).SetValidator(new RecipeValidator());
        RuleFor(recipe => recipe.Instructions).NotEmpty();
        RuleForEach(recipe => recipe.Instructions).SetValidator(new InstructionValidator());
        RuleFor(recipe => recipe.Sections).NotEmpty();
        RuleForEach(recipe => recipe.Sections).SetValidator(new AddSectionValidator());
    }
    public sealed class AddSectionValidator : AbstractValidator<AddSection>
    {
        public AddSectionValidator()
        {
            RuleFor(section => section).SetValidator(new SectionValidator());
            RuleFor(section => section.Ingredients).NotEmpty();
            RuleForEach(section => section.Ingredients).SetValidator(new IngredientValidator());
        }
    }
}
