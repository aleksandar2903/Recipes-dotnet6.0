using MediatR;
using Recipes.Domain.Entities;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Add;

public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Result>
{
    public async Task<Result> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipeId = Guid.NewGuid();
        var recipe = Recipe.Create(
            recipeId,
            request.title,
            request.description,
            request.videoUrl,
            request.thumbnailUrl,
            request.numServings,
            request.totalTimeMinutes,
            request.calories,
            request.sections.Select(section =>
            { 
                var sectionId = Guid.NewGuid();
                return Domain.Entities.Section.Create(
                    sectionId, 
                    recipeId, 
                    section.position, 
                    section.text,
                    section.ingredients.Select(ingredient => 
                    Domain.Entities.Ingredient.Create(
                        Guid.NewGuid(), 
                        sectionId, 
                        ingredient.position, 
                        ingredient.text)).ToList());
            }).ToList(),
            request.instructions.Select(instruction =>
            Domain.Entities.Instruction.Create(
                Guid.NewGuid(),
                recipeId,
                instruction.position,
                instruction.text)).ToList());

        return Result.Success();
    }
}
