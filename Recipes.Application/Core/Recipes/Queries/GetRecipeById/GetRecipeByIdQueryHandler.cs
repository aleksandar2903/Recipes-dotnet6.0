using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Contracts.Queries.Recipes;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, Result<RecipeInformationsResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public GetRecipeByIdQueryHandler(IDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<Result<RecipeInformationsResponse>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        string key = $"recipe-{request.Id}";
        var recipe = await _memoryCache.GetOrCreateAsync(key, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
            return GetRecipeById(request.Id, cancellationToken);
        });
        
        return recipe ?? Result.Failure<RecipeInformationsResponse>(DomainErrors.Recipe.NotFound);
    }

    private async Task<RecipeInformationsResponse> GetRecipeById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Domain.Entities.Recipe>()
            .Where(recipe => recipe.Id == id)
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
                    section.Id,
                    section.Position,
                    section.Text,
                    section.Ingredients.OrderBy(order => order.Position)
                    .Select(ingredient => new IngredientResponse(
                        ingredient.Id,
                        ingredient.Position,
                        ingredient.Text
                        )).ToList()
                    )).ToList(),
                recipe.Instructions.OrderBy(order => order.Position)
                .Select(instruction => new InstructionResponse(
                    instruction.Id,
                    instruction.Position,
                instruction.Text
                )).ToList()
                )).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
