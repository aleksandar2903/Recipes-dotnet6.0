namespace Recipes.Application.Contracts.Queries.Recipes;

public record RecipeInformationsResponse(
    Guid Id, 
    AuthorResponse Author,
    string Title,
    string Description,
    Uri VideoUrl,
    Uri ThumbnailUrl,
    int? NumServings,
    int TotalTimeMinutes,
    int? Calories,
    DateTime CreateOnUtc,
    IReadOnlyCollection<SectionResponse> Sections,
    IReadOnlyCollection<InstructionResponse> Instructions);
public record AuthorResponse(Guid Id, string FullName);
public record IngredientResponse(int Position, string Text);
public record SectionResponse(int Position, string Text, IReadOnlyCollection<IngredientResponse> Ingredients);
public record InstructionResponse(int Position, string Text);
