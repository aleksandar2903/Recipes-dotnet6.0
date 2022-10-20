using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Core.Recipes.Commands.Add;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;

namespace Recipes.Application.Core.Recipes.Commands.Update.Instructions;

public sealed class UpdateInstructionsCommandHandler : IRequestHandler<UpdateInstructionsCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateInstructionsCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Result> Handle(UpdateInstructionsCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        if (!await _dbContext.Set<User>()
            .AnyAsync(user => user.Id == userId, cancellationToken))
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        Recipe recipe = await _dbContext.Set<Recipe>().Where(recipe =>
            recipe.Id == request.RecipeId && recipe.AuthorId == userId)
            .Include(i => i.Instructions)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is null)
        {
            return Result.Failure(DomainErrors.Recipe.RecipeNotFound);
        }
        //Remove exist instructions
        foreach (var instruction in recipe.Instructions.ToList())
        {
            if (!request.Instructions.Any(i => i.Id == instruction.Id))
            {
                recipe.RemoveInstruction(instruction);
            }
        }
        //Create new or update exist instructions
        int position = 0;
        foreach (var instruction in request.Instructions)
        {
            Instruction existsInstruction = recipe.Instructions.FirstOrDefault(i => i.Id == instruction.Id);
            if (existsInstruction is not null)
            {
                existsInstruction.UpdateInformations(++position, instruction.Text);
            }
            else
            {
                recipe.AddInstruction(Guid.Empty, ++position, instruction.Text);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
