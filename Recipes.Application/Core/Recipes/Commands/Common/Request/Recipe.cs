namespace Recipes.Application.Core.Recipes.Commands.Common.Request;

public record Recipe(string Title,
string Description,
Uri? VideoUrl,
Uri ThumbnailUrl,
int? NumServings,
int TotalTimeMinutes,
int? Calories);
public record Section(string Text);
public record Instruction(string Text);
public record Ingredient(string Text);
