using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Contracts.Queries.Recipes;
using Recipes.Domain.Entities;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Queries.GetPopularRecipes;

public class GetPopularRecipesQueryHandler : IRequestHandler<GetPopularRecipesQuery, Result<List<RecipeResponse>>>
{
    private readonly IDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public GetPopularRecipesQueryHandler(IDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }
    public async Task<Result<List<RecipeResponse>>> Handle(GetPopularRecipesQuery request, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(
            "daily-recipes",
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                return GetPopularRecipes(cancellationToken);
            });
    }

    private async Task<List<RecipeResponse>> GetPopularRecipes(CancellationToken cancellationToken = default)
    {
        return await  _dbContext.Set<Recipe>().Select(recipe => new RecipeResponse(
                recipe.Id,
                recipe.Title,
                recipe.VideoUrl,
                recipe.ThumbnailUrl,
                recipe.NumServings,
                recipe.TotalTimeMinutes,
                recipe.Calories,
                recipe.CreatedOnUtc)).AsNoTracking().Take(20).ToListAsync(cancellationToken);
    }
}
