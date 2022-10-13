using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Services.Authentification;
using Recipes.Contracts.Authentification;

namespace Recipes.Api.Controllers.Authentification
{
    [ApiController]
    [Route("auth")]
    public class AuthentificationController : Controller
    {
        IAuthentificationService _authentificationService;

        public AuthentificationController(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            return Ok(request);
        }
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            return Ok(request);
        }
    }
}
