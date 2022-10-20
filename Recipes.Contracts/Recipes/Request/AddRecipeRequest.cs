namespace Recipes.Contracts.Recipes.Request;

public record AddRecipeRequest(
    string title,
    string description,
    Uri videoUrl,
    Uri thumbnailUrl,
    int? numServings,
    int totalTimeMinutes,
    int? calories,
    List<Section> sections,
    List<Instruction> instructions);
public record Section(Guid? Id, int position, string text, List<Ingredient> ingredients);
public record Instruction(Guid? Id, int position, string text);
public record Ingredient(Guid? Id, int position, string text);
