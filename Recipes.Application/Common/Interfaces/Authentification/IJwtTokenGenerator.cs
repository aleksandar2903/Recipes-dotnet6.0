using Recipes.Domain.Entities;

namespace Recipes.Application.Common.Interfaces.Authentification;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
