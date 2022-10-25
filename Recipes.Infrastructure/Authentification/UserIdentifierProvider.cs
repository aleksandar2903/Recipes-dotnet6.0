using Microsoft.AspNetCore.Http;
using Recipes.Application.Common.Interfaces.Authentification;
using System.Security.Claims;

namespace Recipes.Infrastructure.Authentification;

internal sealed class UserIdentifierProvider : IUserIdentifierProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdentifierProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        string userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

        UserId = new Guid(userIdClaim);
    }

    /// <inheritdoc />
    public Guid UserId { get; }
}
