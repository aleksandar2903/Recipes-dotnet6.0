using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Core.Recipes.Commands.Common.Request;

namespace Recipes.Application.Core.Recipes.Commands.Update.Sections;

public record UpdateSectionsCommand(
    Guid RecipeId,
    IReadOnlyCollection<UpdateSection> Sections) : ICommand;
public record UpdateSection(Guid? Id, string Text, IReadOnlyCollection<UpdateIngredient> Ingredients) : Section(Text);
public record UpdateIngredient(Guid? Id, string Text) : Ingredient(Text);
