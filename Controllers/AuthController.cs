using ContactApp.Entities;
using ContactApp.Helpers;
using ContactApp.Services;
using ContactApp.Services.Models.Auth.login;
using ContactApp.Services.Models.Auth.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ContactApp.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController
    {
        private readonly IAuthService _authService;
        

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody]LoginRequestBody requestBody)
        {
            return _authService.Login(requestBody);
        }

        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] RegisterRequestBody requestBody)
        {
            return _authService.Register(requestBody);
        }
    }
}