using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Services.Interfaces;
using StoreApi.Utilities;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(User user)
        {
            var userId = _userService.Register(user.Username, user.PasswordHash, user.Role);
            if (userId == 0)
            {
                return BadRequest("Registration failed.");
            }

            var token = TokenManager.GenerateToken(new User { UserId = userId, Role = user.Role });
            _userService.SaveToken(userId, token);

            return Ok(new { Token = token });
        }
    }
}
