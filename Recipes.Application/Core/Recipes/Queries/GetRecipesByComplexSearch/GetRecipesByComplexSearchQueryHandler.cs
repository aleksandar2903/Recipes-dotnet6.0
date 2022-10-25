using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Contracts.Common;
using Recipes.Application.Contracts.Queries.Recipes;
using Recipes.Domain.Entities;
using Recipes.Domain.Shared;
using System.Linq.Expressions;

namespace Recipes.Application.Core.Recipes.Queries.GetRecipesByComplexSearch;

public sealed class GetRecipesByComplexSearchQueryHandler
    : IRequestHandler<GetRecipeByComplexSearchQuery, Result<PagedList<RecipeResponse>>>
{
    private readonly IDbContext _dbContext;

    public GetRecipesByComplexSearchQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Result<PagedList<RecipeResponse>>> Handle(GetRecipeByComplexSearchQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Recipe> query = _dbContext.Set<Recipe>();

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            query = query.Where(recipe => recipe.Title!.Contains(request.Query));
        }

        if (request.MaxTotalTimeMinutes is not null and > 0)
        {
            query = query.Where(recipe => recipe.TotalTimeMinutes <= request.MaxTotalTimeMinutes);
        }

        if (request.NumServings is not null and > 0)
        {
            query = query.Where(recipe => recipe.NumServings == request.NumServings);
        }

        if (request.MinCalories is not null and > 0)
        {
            query = query.Where(recipe => recipe.Calories >= request.MinCalories);
        }

        if (request.MaxCalories is not null and > 0)
        {
            query = query.Where(recipe => recipe.Calories <= request.MaxCalories);
        }

        if (request.Sort is not null)
        {
            Expression<Func<Recipe, dynamic>> orderExpression = null;
            orderExpression = request.Sort.ToLower() switch
            {
                "title" => s => s.TotalTimeMinutes,
                "calories" => s => s.Calories,
                "totaltimeminutes" => s => s.TotalTimeMinutes,
                "date" => s => s.CreatedOnUtc,
            };

            if (request.SortDirection?.ToLower() == "desc")
                query = query.OrderByDescending(orderExpression);
            else
                query = query.OrderBy(orderExpression);
        }

        var queryDTO = query.Select(recipe => new RecipeResponse(
                recipe.Id,
                recipe.Title,
                recipe.VideoUrl,
                recipe.ThumbnailUrl,
                recipe.NumServings,
                recipe.TotalTimeMinutes,
                recipe.Calories,
                recipe.CreatedOnUtc)).AsNoTracking();

        return await PagedList<RecipeResponse>.ToPagedList(
            queryDTO,
            request.PageNumber ?? 20,
            request.PageSize ?? 1,
            cancellationToken);
    }
}
