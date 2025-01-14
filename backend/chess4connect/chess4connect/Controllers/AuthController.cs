using chess4connect.DTOs;
using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private AuthService _authService;

    public AuthController(AuthService authService) { 
        _authService = authService;
    }


    [HttpPost("signup")]
    public async Task<string> RegisterUserAsync([FromBody] UserSignUpDto receivedUser)
    {
        return await _authService.RegisterUser(receivedUser);
    }


}
