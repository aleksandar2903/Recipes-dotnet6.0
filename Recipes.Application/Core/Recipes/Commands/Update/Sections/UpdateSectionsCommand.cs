using Recipes.Application.Abstractions.Messaging;
using Recipes.Application.Core.Recipes.Commands.Add;

namespace Recipes.Application.Core.Recipes.Commands.Update.Sections;

public record UpdateSectionsCommand(
    string? UserId,
    Guid RecipeId,
    List<SectionRequest> Sections) : ICommand;
