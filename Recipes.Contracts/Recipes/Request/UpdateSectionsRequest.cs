using Recipes.Contracts.Recipes.Request.Common;

namespace Recipes.Contracts.Recipes.Request;

public record UpdateSectionsRequest(IReadOnlyCollection<UpdateSectionRequest> Sections);
public record UpdateSectionRequest(
    Guid? Id,
    string Text,
    IReadOnlyCollection<UpdateIngredientRequest> Ingredients) : SectionRequest(Text);
public record UpdateIngredientRequest(
    Guid? Id,
    string Text) : IngredientRequest(Text);
