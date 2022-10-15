using MediatR;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Authentification.Common;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
using Recipes.Domain.Shared;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Authentification.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthentificationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDbContext _dbContext;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IDbContext dbContext)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
    }
    /// <summary>
    /// Register user and return auth result
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<AuthentificationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        //Validate entered values
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);
        var emailResult = Email.Create(request.Email);
        var passwordResult = Password.Create(request.PasswordHash);

        // Check doesn't user exist in database
        var userExists = await _dbContext.GetByAsync<User>(user => user.Email == emailResult.Value.Value);

        if (userExists is not null)
        {
            return Result.Failure<AuthentificationResult>(DomainErrors.User.DuplicateUser);
        }

        var user = User.Create(Guid.NewGuid(), firstNameResult.Value, lastNameResult.Value, emailResult.Value, passwordResult.Value);

        //Inser user in database

        _dbContext.Insert(user);

        //Save changes

        await _dbContext.SaveChangesAsync(cancellationToken);

        //Inser user in database

        var token = _jwtTokenGenerator.GenerateToken(user);

        return Result.Success(new AuthentificationResult(user, token));
    }
}
