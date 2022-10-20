using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Update;

public sealed class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateRecipeCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Result> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        if (!(await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == userId, cancellationToken)))
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        Recipe recipe = await _dbContext.GetByAsync<Recipe>(recipe =>
            recipe.Id == request.RecipeId && recipe.AuthorId == userId, cancellationToken);
    
        if (recipe is null)
        {
            return Result.Failure(DomainErrors.Recipe.RecipeNotFound);
        }

        recipe.UpdateInformations(request.Title,
                                  request.Description,
                                  request.VideoUrl,
                                  request.ThumbnailUrl,
                                  request.NumServings,
                                  request.TotalTimeMinutes,
                                  request.Calories);
    
        await _dbContext.SaveChangesAsync();
    
        return Result.Success();
    }
}
