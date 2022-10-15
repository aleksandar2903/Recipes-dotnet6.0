using Recipes.Application.Abstractions.Messaging;

namespace Recipes.Application.Core.Recipes.Add;

public record AddRecipeCommand(
    string title,
    string description,
    Uri videoUrl,
    Uri thumbnailUrl,
    int? numServings,
    int? totalTimeMinutes,
    int? calories,
    IReadOnlyCollection<Section> sections,
    IReadOnlyCollection<Instruction> instructions) : ICommand;
public record Section(int position, string text, List<Ingredient> ingredients);
public record Instruction(int position, string text);
public record Ingredient(int position, string text);
