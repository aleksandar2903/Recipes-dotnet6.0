using FluentValidation;
using Recipes.Application.Core.Recipes.Commands.Common.Request;

namespace Recipes.Application.Core.Recipes.Commands.Common.Validation;

public class SectionValidator : AbstractValidator<Section>
{
    public SectionValidator()
    {
        RuleFor(section => section.Text).MaximumLength(150).When(text => !string.IsNullOrWhiteSpace(text.Text));
    }
}

public class IngredientValidator : AbstractValidator<Ingredient>
{
    public IngredientValidator()
    {
        RuleFor(ingredient => ingredient.Text).NotEmpty();
    }
}
public class InstructionValidator : AbstractValidator<Instruction>
{
    public InstructionValidator()
    {
        RuleFor(instruction => instruction.Text).NotEmpty().MaximumLength(500);
    }
}

public class RecipeValidator : AbstractValidator<Recipe>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty();
        RuleFor(recipe => recipe.Description).NotEmpty();
        RuleFor(recipe => recipe.VideoUrl).Must(videoUrl => (videoUrl is null) || (videoUrl.IsAbsoluteUri));
        RuleFor(recipe => recipe.ThumbnailUrl).Must(thumbnaildUrl => thumbnaildUrl.IsAbsoluteUri).NotEmpty();
        RuleFor(recipe => recipe.TotalTimeMinutes).InclusiveBetween(1, 5000);
        RuleFor(recipe => recipe.NumServings).InclusiveBetween(1, 99).When(numServing => numServing is not null);
        RuleFor(recipe => recipe.Calories).Must(calories => (calories is null) || (calories > 0 && calories < 10000));
    }
}
