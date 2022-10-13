namespace Recipes.Contracts.Authentification;

public record LoginRequest(
    string Email, 
    string Password);
