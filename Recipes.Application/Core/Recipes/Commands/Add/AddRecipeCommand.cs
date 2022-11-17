using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Core.Recipes.Commands.Common.Request;

namespace Recipes.Application.Core.Recipes.Commands.Add;

public record AddRecipeCommand(
    string Title,
    string Description,
    Uri? VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories,
    IReadOnlyCollection<AddSection> Sections,
    IReadOnlyCollection<AddInstruction> Instructions) : Recipe(
        Title,
        Description,
        VideoUrl,
        ThumbnailUrl,
        NumServings,
        TotalTimeMinutes,
        Calories), ICommand<Guid>;
public record AddSection(string Text, IReadOnlyCollection<AddIngredient> Ingredients) : Section(Text);
public record AddIngredient(string Text) : Ingredient(Text);
public record AddInstruction(string Text) : Instruction(Text);

