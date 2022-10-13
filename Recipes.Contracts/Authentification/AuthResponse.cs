namespace Recipes.Contracts.Authentification;

public record AuthResponse(
    Guid Id,
    string Email, 
    string Token);
