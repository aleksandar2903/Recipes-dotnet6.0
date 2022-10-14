using MediatR;
using Recipes.Application.Authentification.Common;
using Recipes.Domain.Shared;

namespace Recipes.Application.Authentification.Queries.Login;

public record LoginQuery(string Email, string PasswordHash) : IRequest<Result<AuthentificationResult>>;
