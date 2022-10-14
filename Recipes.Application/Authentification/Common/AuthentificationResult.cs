using Recipes.Domain.Entities;

namespace Recipes.Application.Authentification.Common;

public record AuthentificationResult(User user, string token);
