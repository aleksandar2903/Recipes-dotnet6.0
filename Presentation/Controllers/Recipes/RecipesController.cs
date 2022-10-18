using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Core.Recipes.Commands.Add;
using Recipes.Application.Core.Recipes.Commands.Update.Sections;
using Recipes.Application.Core.Recipes.Queries.GetRecipeById;
using Recipes.Contracts.Recipes.Request;
using System.Security.Claims;

namespace Recipes.Presentation.Controllers.Recipes
{
    [Route("recipes")]
    public class RecipesController : ApiController
    {
        public RecipesController(ISender sender) : base(sender)
        {
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRecipe(AddRecipeRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new AddRecipeCommand(userId, request.title, request.description, request.videoUrl, request.thumbnailUrl, request.numServings, request.totalTimeMinutes, request.calories, request.sections.Select(s => new Application.Core.Recipes.Commands.Add.SectionRequest(s.text, s.ingredients.Select(i => new Application.Core.Recipes.Commands.Add.IngredientRequest(i.text)).ToList())).ToList(), 
                request.instructions.Select(i => new Application.Core.Recipes.Commands.Add.InstructionRequest(i.text)).ToList());
            var result = await Sender.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                var recipeId = result.Value;
                return CreatedAtAction(nameof(GetRecipeById), new { recipeId }, recipeId);
            }
            return HandleFailure(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="cancellationToken"></param>
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
        /// 
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("{recipeId:guid}/sections/update")]
        public async Task<IActionResult> UpdateRecipeSectionsById(Guid recipeId, UpdateSectionsCommand command, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : HandleFailure(result);
        }
    }
}
