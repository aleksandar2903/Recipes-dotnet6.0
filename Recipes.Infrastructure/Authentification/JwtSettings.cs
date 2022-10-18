namespace Recipes.Infrastructure.Authentification;

public class JwtSettings
{
    public string Audience { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public string Secret { get; init; } = null!;
}
