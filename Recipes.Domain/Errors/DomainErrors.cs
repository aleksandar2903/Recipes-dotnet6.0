using Recipes.Domain.Shared;

namespace Recipes.Domain.Errors;

public class DomainErrors
{
    public static class User
    {
        public static readonly Error UserNotFound = new Error("User.NotFound", "User doesn't exist.");
        public static readonly Error DuplicateUser = new Error("User.DuplicateUser", "User already exists.");
        public static readonly Error InvalidPassword = new Error("User.InvalidPassword", "Entered password is invalid.");
    }

    public static class Recipe
    {
        public static readonly Error RecipeNotFound = new Error("Recipe.NotFound", "Recipe with specific Id doesn't exist.");
    }
}
