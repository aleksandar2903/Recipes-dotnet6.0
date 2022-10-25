using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Contracts.Common;
using Recipes.Application.Contracts.Queries.Recipes;

namespace Recipes.Application.Core.Recipes.Queries.GetRecipesByComplexSearch;

public record GetRecipeByComplexSearchQuery(
    string? Query, 
    int? NumServings, 
    int? MaxTotalTimeMinutes, 
    int? MinCalories, 
    int? MaxCalories, 
    string? Sort, 
    string? SortDirection, 
    int? PageNumber = 1, 
    int? PageSize = 20) 
    : IQuery<PagedList<RecipeResponse>>;
