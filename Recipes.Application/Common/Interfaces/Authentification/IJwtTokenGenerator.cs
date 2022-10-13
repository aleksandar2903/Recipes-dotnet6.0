namespace Recipes.Application.Common.Interfaces.Authentification;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string firstName, string lastName);
}
