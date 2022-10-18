using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Update.Sections;

public sealed class UpdateSectionsCommandHandler : IRequestHandler<UpdateSectionsCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateSectionsCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Result> Handle(UpdateSectionsCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        User user = await _dbContext.GetByIdAsync<User>(userId);

        if (user is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        bool recipeExists = await _dbContext.Set<Recipe>().AnyAsync(recipe =>
            recipe.Id == request.RecipeId && recipe.AuthorId == user.Id);

        if (!recipeExists)
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        await _dbContext.BeginTransactionAsync();

        Guid sectionId = Guid.NewGuid();
        int sectionPosition = 0;
        int ingredientPosition = 0;
        var sections = request.Sections.Select(section =>
            Section.Create(
                sectionId,
                request.RecipeId,
                ++sectionPosition,
                section.Text,
                section.Ingredients
                .Select(ingredient =>
                    Ingredient.Create(
                        Guid.NewGuid(),
                        sectionId,
                        ++ingredientPosition,
                        ingredient.Text)).ToList()
                )).ToList();

        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Section WHERE RecipeId = {request.RecipeId}");
        await _dbContext.Set<Section>().AddRangeAsync(sections);

        await _dbContext.SaveChangesAsync();
        await _dbContext.CommitTransactionAsync();

        return Result.Success();
    }
}
