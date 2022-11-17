using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Shared;

namespace Recipes.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiController : Controller
    {
        protected ISender Sender;

        public ApiController(ISender sender)
        {
            Sender = sender;
        }

        protected IActionResult HandleFailure(Result result)
        {
            return result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),
                { Error.Code: "Recipe.NotFound"} => NotFound(
                    CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, result.Error)),
                IValidateResult validateResult =>
                    BadRequest(
                        CreateProblemDetails("Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validateResult.Errors)
                        ),
                _ => BadRequest(
                        CreateProblemDetails("Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error)
                        )
            };
        }

        private ProblemDetails CreateProblemDetails(string title, 
            int? status, 
            Error error, 
            Error[] errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Status = status,
                Detail = error.Message,
                Extensions = { { nameof(errors), errors } }
            };
    }
}
