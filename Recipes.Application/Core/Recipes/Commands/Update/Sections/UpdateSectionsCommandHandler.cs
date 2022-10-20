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

        if (!(await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == userId, cancellationToken)))
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        //Recipe recipe = await _dbContext.Set<Recipe>().FirstOrDefaultAsync(recipe => recipe.Id == request.RecipeId && recipe.AuthorId == userId);
        Recipe recipe = await _dbContext.Set<Recipe>().Where(recipe => recipe.Id == request.RecipeId && recipe.AuthorId == userId)
            .Include(sections => sections.Sections)
            .ThenInclude(ingredients => ingredients.Ingredients)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is null)
        {
            return Result.Failure(DomainErrors.Recipe.RecipeNotFound);
        }

        //Remove exist instructions with ingredients
        foreach (var section in recipe.Sections.ToList())
        {
            if (!request.Sections.Any(i => i.Id == section.Id))
            {
                recipe.RemoveSection(section);
            }
        }
        //Create new or update exist section with ingredients
        int sectionPosition = 0;
        foreach (var section in request.Sections)
        {
            Section existsSection = recipe.Sections.FirstOrDefault(i => i.Id == section.Id);
            if (existsSection is not null)
            {
                existsSection.UpdateInformations(++sectionPosition, section.Text);
                foreach (var ingredient in existsSection.Ingredients.ToList())
                {
                    if (!section.Ingredients.Any(i => i.Id == ingredient.Id))
                    {
                        existsSection.RemoveIngredient(ingredient);
                    }
                }

                int ingredientPosition = 0;
                foreach (var ingredient in section.Ingredients)
                {
                    Ingredient existsIngredient = existsSection.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
                    if (existsIngredient is not null)
                    {
                        existsIngredient.UpdateInformations(++ingredientPosition, ingredient.Text);
                    }
                    else
                    {
                        existsSection.AddIngredient(Guid.Empty, ++ingredientPosition, ingredient.Text);
                    }
                }
            }
            else
            {
                int ingredientPosition = 0;
                recipe.AddSection(Guid.Empty,
                    ++sectionPosition,
                    section.Text,
                    section.Ingredients.Select(ingredient =>
                        Ingredient.Create(
                                Guid.Empty,
                                Guid.Empty,
                                ++ingredientPosition,
                                ingredient.Text)
                        ).ToList());
            }
        }

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
