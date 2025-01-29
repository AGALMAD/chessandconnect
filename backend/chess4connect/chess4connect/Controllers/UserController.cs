using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private UserService _userService;

        public UserController(UserService userService) 
        { 
            _userService = userService;
        }

        [HttpGet]
        public async Task<User> GetAuthenticatedUser()
        {
            //Si no es una usuario autenticado termina la ejecución
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }

            User user = await _userService.GetUserById(Int32.Parse(userId));
            return await _userService.GetUserById(Int32.Parse(userId));

        }



    }
}
