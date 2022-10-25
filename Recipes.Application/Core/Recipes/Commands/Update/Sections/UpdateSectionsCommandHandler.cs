using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Update.Sections;

public sealed class UpdateSectionsCommandHandler : IRequestHandler<UpdateSectionsCommand, Result>
{
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IDbContext _dbContext;

    public UpdateSectionsCommandHandler(IDbContext dbContext, IUserIdentifierProvider userIdentifierProvider)
    {
        _dbContext = dbContext;
        _userIdentifierProvider = userIdentifierProvider;
    }
    public async Task<Result> Handle(UpdateSectionsCommand request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == _userIdentifierProvider.UserId, cancellationToken)))
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        //Recipe recipe = await _dbContext.Set<Recipe>().FirstOrDefaultAsync(recipe => recipe.Id == request.RecipeId && recipe.AuthorId == userId);
        Recipe recipe = await _dbContext.Set<Recipe>().Where(recipe => 
        recipe.Id == request.RecipeId && 
        recipe.AuthorId == _userIdentifierProvider.UserId)
            .Include(sections => sections.Sections)
            .ThenInclude(ingredients => ingredients.Ingredients)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is null)
        {
            return Result.Failure(DomainErrors.Recipe.NotFound);
        }

        recipe.UpdateSections(request.Sections.Select(section =>
                    Domain.ValueObjects.Section.Create(
                    section.Id,
                    section.Text,
                    section.Ingredients.Select(ingredient =>
                        Domain.ValueObjects.Ingredient.Create(
                                ingredient.Id,
                                ingredient.Text).Value
                        ).ToList()).Value).ToList());

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
