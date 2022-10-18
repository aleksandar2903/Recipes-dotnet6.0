using FluentValidation;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public class AddRecipeCommandValidator : AbstractValidator<AddRecipeCommand>
{
    public AddRecipeCommandValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty();
        RuleFor(recipe => recipe.Description).NotEmpty();
        RuleFor(recipe => recipe.VideoUrl).Must(videoUrl => (videoUrl is null) || (videoUrl.IsAbsoluteUri));
        RuleFor(recipe => recipe.ThumbnailUrl).Must(thumbnaildUrl => thumbnaildUrl.IsAbsoluteUri).NotEmpty();
        RuleFor(recipe => recipe.TotalTimeMinutes).InclusiveBetween(1, 5000);
        RuleFor(recipe => recipe.NumServings).InclusiveBetween(1, 99).When(numServing => numServing is not null);
        RuleFor(recipe => recipe.Calories).Must(calories => (calories is null) || (calories > 0 && calories < 10000));
        RuleFor(recipe => recipe.Instructions).NotEmpty();
        RuleForEach(recipe => recipe.Instructions).SetValidator(new AddInstructionValidation());
        RuleFor(recipe => recipe.Sections).NotEmpty();
        RuleForEach(recipe => recipe.Sections).SetValidator(new AddSectionValidation());
    }
}
public class AddSectionValidation : AbstractValidator<SectionRequest>
{
    public AddSectionValidation()
    {
        RuleFor(section => section.Ingredients).NotEmpty();
        RuleForEach(section => section.Ingredients).SetValidator(new AddIngredientValidation());
    }
}

public class AddIngredientValidation : AbstractValidator<IngredientRequest>
{
    public AddIngredientValidation()
    {
        RuleFor(ingredient => ingredient.Text).NotEmpty();
    }
}
public class AddInstructionValidation : AbstractValidator<InstructionRequest>
{
    public AddInstructionValidation()
    {
        RuleFor(instruction => instruction.Text).NotEmpty().MaximumLength(500);
    }
}
