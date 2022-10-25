using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;
using InstructionValueObject =  Recipes.Domain.ValueObjects.Instruction;

namespace Recipes.Application.Core.Recipes.Commands.Update.Instructions;

public sealed class UpdateInstructionsCommandHandler : IRequestHandler<UpdateInstructionsCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public UpdateInstructionsCommandHandler(IDbContext dbContext, IUserIdentifierProvider userIdentifierProvider)
    {
        _dbContext = dbContext;
        _userIdentifierProvider = userIdentifierProvider;
    }
    public async Task<Result> Handle(UpdateInstructionsCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == _userIdentifierProvider.UserId, cancellationToken))
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        Recipe recipe = await _dbContext.Set<Recipe>().Where(recipe =>
            recipe.Id == request.RecipeId && recipe.AuthorId == _userIdentifierProvider.UserId)
            .Include(i => i.Instructions)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is null)
        {
            return Result.Failure(DomainErrors.Recipe.NotFound);
        }

        recipe.UpdateInstructions(request.Instructions.
            Select(instruction =>
            InstructionValueObject.Create(
                instruction.Id,
                instruction.Text).Value).ToList());

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
