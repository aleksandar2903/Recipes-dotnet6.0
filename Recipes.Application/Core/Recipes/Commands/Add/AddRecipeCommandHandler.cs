using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Result<Guid>>
{
    private readonly IDbContext _dbContext;

    public AddRecipeCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
        {
            return Result.Failure<Guid>(DomainErrors.User.UserNotFound);
        }

        if (!(await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == userId, cancellationToken)))
        {
            return Result.Failure<Guid>(DomainErrors.User.UserNotFound);
        }

        var recipe = MapRecipeRequestToDomainEntity(request, userId);

        _dbContext.Insert(recipe);

        await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success(recipe.Id);
    }

    private Recipe MapRecipeRequestToDomainEntity(AddRecipeCommand request, Guid userId)
    {
        int sectionPosition = 0;
        int instructionPosition = 0;
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
            {
                int ingredientPosition = 0;
                return Section.Create(
                    Guid.Empty,
                    Guid.Empty,
                    ++sectionPosition,
                    section.Text,
                    section.Ingredients.Select(ingredient => Ingredient.Create(
                        Guid.Empty,
                        Guid.Empty,
                        ++ingredientPosition,
                        ingredient.Text)).ToList());
            }).ToList(),
            request.Instructions.Select(instruction =>
                Instruction.Create(
                    Guid.Empty,
                    Guid.Empty,
                    ++instructionPosition,
                    instruction.Text)).ToList());
    }
}
