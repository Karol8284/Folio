using Folio.CORE.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Folio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (success, message) = await _authService.RegisterAsync(
            request.Email, request.Password, request.FullName);

        if (!success)
            return BadRequest(new { message });

        return Ok(new { message = "Rejestracja powiodła się" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, token, message) = await _authService.LoginAsync(
            request.Email, request.Password);

        if (!success)
            return Unauthorized(new { message });

        return Ok(new { token, message });
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}