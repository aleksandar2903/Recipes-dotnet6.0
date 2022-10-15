using Recipes.Domain.Primitives;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities;

public sealed class User : Entity, IAuditableEntity
{
    private User(Guid id, string firstName, string lastName, string email, string passwordHash) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }

    public static User Create(Guid id, FirstName firstName, LastName lastName, Email email, Password passwordHash)
    {
        return new User(id, firstName.Value, lastName.Value, email.Value, passwordHash.Value);
    }

    public bool IsPasswordMatched(Password passwordHash)
    {
        return PasswordHash == passwordHash.Value;
    }
}
