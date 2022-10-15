using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Shared;
using System.Collections;

namespace Recipes.Api.Controllers
{
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
