using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Authentification.Common;

namespace Recipes.Application.Authentification.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string PasswordHash) : ICommand<AuthentificationResult>;
