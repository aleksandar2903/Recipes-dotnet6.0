using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Authentification.Queries.Login;
using Recipes.Application.Core.Recipes.Add;
using Recipes.Contracts.Authentification;
using Recipes.Contracts.Recipes.Request;
using Recipes.Infrastructure;

namespace Recipes.Api.Controllers.Recipes
{
    [Route("recipes")]
    public class RecipesController : ApiController
    {
        public RecipesController(ISender sender) : base(sender)
        {
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRecipe(AddRecipeRequest request)
        {
            var command = new AddRecipeCommand(request.title, request.description, request.videoUrl, request.thumbnailUrl, request.numServings, request.totalTimeMinutes, request.calories, request.sections.Select(s => new Application.Core.Recipes.Add.Section(s.position, s.text, s.ingredients.Select(i => new Application.Core.Recipes.Add.Ingredient(i.position, i.text)).ToList())).ToList(), 
                request.instructions.Select(i => new Application.Core.Recipes.Add.Instruction(i.position, i.text)).ToList());
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok(result) : HandleFailure(result);
        }
    }
}
