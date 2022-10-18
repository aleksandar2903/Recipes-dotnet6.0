using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Contracts.Queries.Recipes;

namespace Recipes.Application.Core.Recipes.Queries.GetRecipeById;

public record GetRecipeByIdQuery(Guid Id) : IQuery<RecipeInformationsResponse>;
