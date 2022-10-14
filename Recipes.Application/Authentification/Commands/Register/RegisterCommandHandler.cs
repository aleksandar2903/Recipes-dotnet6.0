using MediatR;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Authentification.Common;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Application.Persistance;
using Recipes.Domain.Entities;
using Recipes.Domain.Shared;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Authentification.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthentificationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IDbContext _dbContext;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IDbContext dbContext)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
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
        // Check doesn't user exist in database
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);
        var emailResult = Email.Create(request.Email);

        var result = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult);

        if (result.IsFailure)
        {
            return Result.Failure<AuthentificationResult>(result.Error);
        }
        var userExists = await _userRepository.GetUserByEmailAsync(emailResult.Value.Value);

        if (userExists is not null)
        {
            return Result.Failure<AuthentificationResult>(new Error("AuthentificationResult.DuplicateUser", "User already exists"));
        }


        var user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, request.PasswordHash);

        _dbContext.Set<User>().Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return Result.Success(new AuthentificationResult(user, token));
    }
}
