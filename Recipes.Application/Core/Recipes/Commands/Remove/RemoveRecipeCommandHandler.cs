using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Remove
{
    public sealed class RemoveRecipeCommandHandler : IRequestHandler<RemoveRecipeCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        public RemoveRecipeCommandHandler(IDbContext dbContext, IUserIdentifierProvider userIdentifierProvider)
        {
            _dbContext = dbContext;
            _userIdentifierProvider = userIdentifierProvider;
        }
        public async Task<Result> Handle(RemoveRecipeCommand request, CancellationToken cancellationToken)
        {
            if (!(await _dbContext.Set<User>()
                .AnyAsync(user => user.Id == _userIdentifierProvider.UserId, cancellationToken)))
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Recipe recipe = await _dbContext.GetByAsync<Recipe>(recipe =>
            recipe.Id == request.RecipeId && recipe.AuthorId == _userIdentifierProvider.UserId, cancellationToken);

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
