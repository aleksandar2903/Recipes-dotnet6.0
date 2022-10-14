using MediatR;
using Recipes.Application.Authentification.Common;
using Recipes.Domain.Shared;

namespace Recipes.Application.Authentification.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string PasswordHash) : IRequest<Result<AuthentificationResult>>;
