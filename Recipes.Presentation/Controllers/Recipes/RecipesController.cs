using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Core.Recipes.Commands.Add;
using Recipes.Application.Core.Recipes.Commands.Remove;
using Recipes.Application.Core.Recipes.Commands.Update;
using Recipes.Application.Core.Recipes.Commands.Update.Instructions;
using Recipes.Application.Core.Recipes.Commands.Update.Sections;
using Recipes.Application.Core.Recipes.Queries.GetPopularRecipes;
using Recipes.Application.Core.Recipes.Queries.GetRecipeById;
using Recipes.Application.Core.Recipes.Queries.GetRecipesByComplexSearch;
using Recipes.Contracts.Recipes.Request;
using System.Security.Claims;

namespace Recipes.Presentation.Controllers.Recipes;

/// <summary>
/// Represents the recipes controller
/// </summary>
[Route("recipes")]
public class RecipesController : ApiController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    public RecipesController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    /// Gets the recipe with the specific identifier.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{recipeId:guid}")]
    public async Task<IActionResult> GetRecipeById(Guid recipeId, CancellationToken cancellationToken)
    {
        var query = new GetRecipeByIdQuery(recipeId);
        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
    /// <summary>
    /// Gets the recipe with the specific identifier.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetRecipeByComplexSearch(
        string? query,
        int? numServings,
        int? maxTotalTimeMinutes,
        int? minCalories,
        int? maxCalories,
        string? sort,
        string? sortDirection,
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var recipeQuery = new GetRecipeByComplexSearchQuery(
            query,
            numServings,
            maxTotalTimeMinutes,
            minCalories,
            maxCalories,
            sort,
            sortDirection,
            pageNumber,
            pageSize);
        var result = await Sender.Send(recipeQuery, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
    [AllowAnonymous]
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularRecipes(
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetPopularRecipesQuery(), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
    /// <summary>
    /// Create a new recipe base on the specific request.
    /// </summary>
    /// <param name="request">The create recipe request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddRecipe(AddRecipeRequest request, CancellationToken cancellationToken)
    {
        var command = new AddRecipeCommand(
            request.Title,
            request.Description,
            request.VideoUrl,
            request.ThumbnailUrl,
            request.NumServings,
            request.TotalTimeMinutes,
            request.Calories,
            request.Sections.Select(s => new AddSection(
                s.Text, 
                s.Ingredients.Select(i => new AddIngredient(i.Text)).ToList())).ToList(),
            request.Instructions.Select(i => new AddInstruction(i.Text)).ToList());
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            var recipeId = result.Value;
            return CreatedAtAction(nameof(GetRecipeById), new { recipeId }, recipeId);
        }
        return HandleFailure(result);
    }
    /// <summary>
    /// Patches the recipe with specific identifier based on the specific request.
    /// </summary>
    /// <param name="command">The update recipe request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpPatch("{recipeId:guid}")]
    public async Task<IActionResult> UpdateRecipe(Guid recipeId, UpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRecipeCommand(
            recipeId,
            request.Title,
            request.Description,
            request.VideoUrl,
            request.ThumbnailUrl,
            request.NumServings,
            request.TotalTimeMinutes,
            request.Calories);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : HandleFailure(result);
    }
    /// <summary>
    /// Patch sections with specific recipe identfier based on the specific request.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="request">The recipe identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpPatch("{recipeId:guid}/sections")]
    public async Task<IActionResult> UpdateRecipeSections(Guid recipeId, UpdateSectionsRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSectionsCommand(
            recipeId,
            request.Sections.Select(s => new UpdateSection(
                s.Id, 
                s.Text, 
                s.Ingredients.Select(i => new UpdateIngredient(i.Id, i.Text)).ToList())).ToList());
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : HandleFailure(result);
    }
    /// <summary>
    /// Patch instructions with specific recipe identfier based on the specific request.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpPatch("{recipeId:guid}/instructions")]
    public async Task<IActionResult> UpdateRecipeInstructions(Guid recipeId, UpdateInstructionsRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateInstructionsCommand(
            recipeId,
            request.Instructions.Select(s => new UpdateInstruction(
                s.Id,
                s.Text)).ToList());
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : HandleFailure(result);
    }
    /// <summary>
    /// Delete the recipe with the specific identitfier.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpDelete("{recipeId:guid}")]
    public async Task<IActionResult> DeleteRecipe(Guid recipeId, CancellationToken cancellationToken)
    {
        var command = new RemoveRecipeCommand(recipeId);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : HandleFailure(result);
    }
}
