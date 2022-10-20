using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Core.Recipes.Commands.Add;
using Recipes.Application.Core.Recipes.Commands.Remove;
using Recipes.Application.Core.Recipes.Commands.Update;
using Recipes.Application.Core.Recipes.Commands.Update.Instructions;
using Recipes.Application.Core.Recipes.Commands.Update.Sections;
using Recipes.Application.Core.Recipes.Queries.GetRecipeById;
using Recipes.Application.Core.Recipes.Queries.GetRecipesByComplexSearch;
using Recipes.Contracts.Recipes.Request;
using System.Security.Claims;

namespace Recipes.Presentation.Controllers.Recipes
{
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
            string? queryString,
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
            var query = new GetRecipeByComplexSearchQuery(
                queryString, 
                numServings, 
                maxTotalTimeMinutes, 
                minCalories,
                maxCalories,
                sort,
                sortDirection,
                pageNumber ?? 1,
                pageSize ?? 20);
            var result = await Sender.Send(query, cancellationToken);

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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new AddRecipeCommand(userId, request.title, request.description, request.videoUrl, request.thumbnailUrl, request.numServings, request.totalTimeMinutes, request.calories, request.sections.Select(s => new Application.Core.Recipes.Commands.Add.SectionRequest(s.Id, s.text, s.ingredients.Select(i => new Application.Core.Recipes.Commands.Add.IngredientRequest(i.Id, i.text)).ToList())).ToList(), 
                request.instructions.Select(i => new Application.Core.Recipes.Commands.Add.InstructionRequest(i.Id, i.text)).ToList());
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
        public async Task<IActionResult> UpdateRecipe(UpdateRecipeCommand command, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        public async Task<IActionResult> UpdateRecipeSections(Guid recipeId, UpdateSectionsCommand command, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        public async Task<IActionResult> UpdateRecipeInstructions(Guid recipeId, UpdateInstructionsCommand command, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new RemoveRecipeCommand(userId, recipeId);
            var result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : HandleFailure(result);
        }
    }
}
