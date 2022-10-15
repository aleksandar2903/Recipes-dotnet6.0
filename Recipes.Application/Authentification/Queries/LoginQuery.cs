using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Authentification.Common;

namespace Recipes.Application.Authentification.Queries.Login;

public record LoginQuery(string Email, string PasswordHash) : IQuery<AuthentificationResult>;
