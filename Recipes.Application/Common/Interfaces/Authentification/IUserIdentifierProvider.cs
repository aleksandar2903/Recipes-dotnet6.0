namespace Recipes.Application.Common.Interfaces.Authentification;

public interface IUserIdentifierProvider
{
    /// <summary>
    /// Gets the authenticated user identifier.
    /// </summary>
    Guid UserId { get; }
}
