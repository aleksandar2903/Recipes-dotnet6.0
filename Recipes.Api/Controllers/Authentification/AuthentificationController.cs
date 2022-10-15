using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Authentification.Commands.Register;
using Recipes.Application.Authentification.Queries.Login;
using Recipes.Contracts.Authentification;

namespace Recipes.Api.Controllers.Authentification
{
    /// <summary>
    /// 
    /// </summary>
    [Route("auth")]
    public class AuthentificationController : ApiController
    {
        public AuthentificationController(ISender sender) : base(sender)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = new LoginQuery(request.Email, request.Password);
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }
    }
}
