using MediatR;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Authentification.Common;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Domain.Entities;
using Recipes.Domain.Errors;
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
        var emailResult = Email.Create(request.Email);
        var passwordResult = Password.Create(request.PasswordHash);
        // Check does user exist in database
        var user = await _dbContext.GetByAsync<User>(user => user.Email == emailResult.Value.Value);

        if (user is null)
        {
            return Result.Failure<AuthentificationResult>(DomainErrors.User.UserNotFound);
        }

        // Check does password match user password in database

        if (!user.IsPasswordMatched(passwordResult.Value))
        {
            return Result.Failure<AuthentificationResult>(DomainErrors.User.InvalidPassword);
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthentificationResult(user, token);
    }
}
