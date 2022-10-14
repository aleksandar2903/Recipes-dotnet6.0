using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Authentification.Common;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Application.Persistance;
using Recipes.Domain.Entities;
using Recipes.Domain.Shared;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Authentification.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthentificationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDbContext _dbContext;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IDbContext dbContext)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
    }

    public async Task<Result<AuthentificationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // Check does user exists in database
        var email = Email.Create(request.Email);

        var result = Result.FirstFailureOrSuccess(email);

        if (result.IsFailure)
        {
            return Result.Failure<AuthentificationResult>(result.Error);
        }

        var user = await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.Email == email.Value.Value);

        if (user is null)
        {
            return Result.Failure<AuthentificationResult>(new Error("User.NotFound", "User doesn't exits."));
        }

        // Check does password matches user password in database

        if (user.PasswordHash != request.PasswordHash)
        {
            return Result.Failure<AuthentificationResult>(new Error("User.InvalidPassword", "Entered password is invalid."));
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthentificationResult(user, token);
    }
}
