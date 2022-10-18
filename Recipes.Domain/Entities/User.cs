using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;
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

    private readonly List<Recipe> _recipes = new();
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public IReadOnlyCollection<Recipe> Recipes => _recipes;
    public static User Create(Guid id, FirstName firstName, LastName lastName, Email email, Password passwordHash)
    {
        return new User(id, firstName.Value, lastName.Value, email.Value, passwordHash.Value);
    }

    public bool IsPasswordMatched(Password passwordHash)
    {
        return PasswordHash == passwordHash.Value;
    }

    public Recipe AddRecipe(
        Guid id,
        string title,
        string description,
        Uri videoUrl,
        Uri thumbnailUrl,
        int? numServings,
        int totalTimeMinutes,
        int? calories,
        List<Section> sections,
        List<Instruction> instructions)
    {
        var recipe = Recipe.Create(
            id, 
            this, 
            title,
            description, 
            videoUrl, 
            thumbnailUrl, 
            numServings, 
            totalTimeMinutes, 
            calories, 
            sections, 
            instructions);

        _recipes.Add(recipe);

        return recipe;
    }
}
