using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Contracts.Queries.Recipes;

namespace Recipes.Application.Core.Recipes.Queries.GetPopularRecipes;

public record GetPopularRecipesQuery() : IQuery<List<RecipeResponse>>;
