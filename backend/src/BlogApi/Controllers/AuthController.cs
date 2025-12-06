

using BlogApi.Data;
using BlogApi.Dtos.Auth;
using BlogApi.Dtos.Common;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public ActionResult Register(RegisterRequestDto registerDto)
    {
        return Ok(ApiResponse<AuthResponseDto>.Ok(_authService.Register(registerDto)));
    }

    [HttpPost("login")]
    public ActionResult Login(LoginRequestDto loginDto)
    {
        return Ok(ApiResponse<AuthResponseDto>.Ok(_authService.Login(loginDto)));
    }

}