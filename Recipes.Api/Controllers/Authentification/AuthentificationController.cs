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
    [ApiController]
    [Route("auth")]
    public class AuthentificationController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ISender _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public AuthentificationController(ISender mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = new LoginQuery(request.Email, request.Password);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
