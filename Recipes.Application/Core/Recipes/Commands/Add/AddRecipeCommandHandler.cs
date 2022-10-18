using MediatR;
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

        User user = await _dbContext.GetByIdAsync<User>(userId);

        if(user is null)
        {
            return Result.Failure<Guid>(DomainErrors.User.UserNotFound);
        }

        Guid recipeId = Guid.NewGuid();
        Guid sectionId = Guid.NewGuid();
        int sectionPosition = 0;
        int instructionPosition = 0;

        var recipe = user.AddRecipe(
            recipeId,
            request.Title,
            request.Description,
            request.VideoUrl,
            request.ThumbnailUrl,
            request.NumServings,
            request.TotalTimeMinutes,
            request.Calories,
            request.Sections.Select(section =>
            {
                Guid sectionId = Guid.NewGuid();
                int ingredientPosition = 0;
                return Section.Create(
                    sectionId,
                    recipeId,
                    sectionPosition++,
                    section.Text,
                    section.Ingredients.Select(ingredient => Ingredient.Create(
                        Guid.NewGuid(), 
                        sectionId, 
                        ingredientPosition, 
                        ingredient.Text)).ToList());
            }).ToList(),
            request.Instructions.Select(instruction =>
                Instruction.Create(
                    Guid.NewGuid(),
                    recipeId,
                    instructionPosition++,
                    instruction.Text)).ToList());

        _dbContext.Insert(recipe);

        await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success(recipeId);
    }
}
