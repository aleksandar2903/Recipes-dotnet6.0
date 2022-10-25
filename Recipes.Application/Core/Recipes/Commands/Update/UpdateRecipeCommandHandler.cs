using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Update;

public sealed class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Result>
{
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IDbContext _dbContext;

    public UpdateRecipeCommandHandler(IDbContext dbContext, IUserIdentifierProvider userIdentifierProvider)
    {
        _dbContext = dbContext;
        _userIdentifierProvider = userIdentifierProvider;
    }
    public async Task<Result> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == _userIdentifierProvider.UserId, cancellationToken)))
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        Recipe recipe = await _dbContext.GetByAsync<Recipe>(recipe =>
            recipe.Id == request.RecipeId && 
            recipe.AuthorId == _userIdentifierProvider.UserId, cancellationToken);
    
        if (recipe is null)
        {
            return Result.Failure(DomainErrors.Recipe.NotFound);
        }

        recipe.UpdateInformations(request.Title,
                                  request.Description,
                                  request.VideoUrl,
                                  request.ThumbnailUrl,
                                  request.NumServings,
                                  request.TotalTimeMinutes,
                                  request.Calories);
    
        await _dbContext.SaveChangesAsync(cancellationToken);
    
        return Result.Success();
    }
}
