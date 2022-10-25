using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Result<Guid>>
{
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IDbContext _dbContext;

    public AddRecipeCommandHandler(IDbContext dbContext, IUserIdentifierProvider userIdentifierProvider)
    {
        _dbContext = dbContext;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<Guid>> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == _userIdentifierProvider.UserId, cancellationToken)))
        {
            return Result.Failure<Guid>(DomainErrors.User.NotFound);
        }

        var recipe = MapRecipeRequestToDomainEntity(request, _userIdentifierProvider.UserId);

        _dbContext.Insert(recipe);

        await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success(recipe.Id);
    }

    private Recipe MapRecipeRequestToDomainEntity(AddRecipeCommand request, Guid userId)
    {
        return Recipe.Create(
            Guid.Empty,
            userId,
            request.Title,
            request.Description,
            request.VideoUrl,
            request.ThumbnailUrl,
            request.NumServings,
            request.TotalTimeMinutes,
            request.Calories,
            request.Sections.Select(section =>
                Domain.ValueObjects.Section.Create(
                    Guid.Empty,
                    section.Text,
                    section.Ingredients.Select(ingredient => Domain.ValueObjects.Ingredient.Create(
                        Guid.Empty,
                        ingredient.Text).Value).ToList()).Value).ToList(),
            request.Instructions.Select(instruction =>
                Domain.ValueObjects.Instruction.Create(
                    Guid.Empty,
                    instruction.Text).Value).ToList());
    }
}
