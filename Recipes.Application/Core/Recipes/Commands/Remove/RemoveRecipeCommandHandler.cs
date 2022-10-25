using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Remove
{
    public sealed class RemoveRecipeCommandHandler : IRequestHandler<RemoveRecipeCommand, Result>
    {
        private readonly IDbContext _dbContext;
        public RemoveRecipeCommandHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result> Handle(RemoveRecipeCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out Guid userId))
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            if (!(await _dbContext.Set<User>()
                .AnyAsync(user => user.Id == userId, cancellationToken)))
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Recipe recipe = await _dbContext.GetByAsync<Recipe>(recipe =>
            recipe.Id == request.RecipeId && recipe.AuthorId == userId, cancellationToken);

            if (recipe is null)
            {
                return Result.Failure(DomainErrors.Recipe.NotFound);
            }

            _dbContext.Remove(recipe);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
