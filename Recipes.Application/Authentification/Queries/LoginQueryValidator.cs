using FluentValidation;
using Recipes.Application.Authentification.Queries.Login;

namespace Recipes.Application.Authentification.Queries;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
	public LoginQueryValidator()
	{
		RuleFor(user => user.Email).EmailAddress().NotEmpty();
		RuleFor(user => user.PasswordHash).NotEmpty();
	}
}
