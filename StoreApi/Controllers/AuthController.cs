using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Services.Interfaces;
using StoreApi.Utilities;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Authenticate(User user)
        {
            var authenticatedUser = _authService.Authenticate(user.Username, user.PasswordHash);
            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var token = _authService.GetToken(authenticatedUser.UserId);
            if (token == null)
            {
                token = TokenManager.GenerateToken(authenticatedUser);
                _authService.SaveToken(authenticatedUser.UserId, token);
            }

            return Ok(new { Token = token });
        }
    }
}
