using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Contracts.Queries.Recipes;
using Recipes.Domain.Entities;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, Result<RecipeInformationsResponse>>
{
    private readonly IDbContext _dbContext;

    public GetRecipeByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<RecipeInformationsResponse>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        var recipe = await _dbContext.Set<Recipe>()
            .Where(recipe => recipe.Id == request.Id)
            .Select(recipe => new RecipeInformationsResponse(
                recipe.Id,
                new AuthorResponse(
                    recipe.AuthorId,
                    $"{recipe.Author.FirstName} {recipe.Author.LastName}"),
                recipe.Title,
                recipe.Description,
                recipe.VideoUrl,
                recipe.ThumbnailUrl,
                recipe.NumServings,
                recipe.TotalTimeMinutes,
                recipe.Calories,
                recipe.CreatedOnUtc,
                recipe.Sections.OrderBy(order => order.Position)
                .Select(section => new SectionResponse(
                    section.Position,
                    section.Text,
                    section.Ingredients.OrderBy(order => order.Position)
                    .Select(ingredient => new IngredientResponse(
                        ingredient.Position,
                        ingredient.Text
                        )).ToList()
                    )).ToList(),
                recipe.Instructions.OrderBy(order => order.Position)
                .Select(instruction => new InstructionResponse(
                    instruction.Position,
                    instruction.Text
                    )).ToList()
                )).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if(recipe is null)
        {
            return Result.Failure<RecipeInformationsResponse>(new Error("Recipe.NotFound", "Recipe doesn't exist."));
        }

        return recipe;
    }
}
