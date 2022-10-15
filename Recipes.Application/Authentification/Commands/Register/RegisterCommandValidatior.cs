using FluentValidation;
using Recipes.Domain.ValueObjects;
using System.Data;

namespace Recipes.Application.Authentification.Commands.Register;

public class RegisterCommandValidatior : AbstractValidator<RegisterCommand>
{
	public RegisterCommandValidatior()
	{
		RuleFor(user => user.FirstName).NotEmpty().MaximumLength(FirstName.MaxLenght);
		RuleFor(user => user.LastName).NotEmpty().MaximumLength(LastName.MaxLenght);
		RuleFor(user => user.Email).EmailAddress().MaximumLength(Email.MaxLenght);
		RuleFor(user => user.PasswordHash).NotEmpty();
	}
}
