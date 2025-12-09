

using BlogApi.Data;
using BlogApi.Dtos.Auth;
using BlogApi.Dtos.Common;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public ActionResult Register(RegisterRequestDto registerDto)
    {
        var result = _authService.Register(registerDto);
        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = result.RefreshTokenExpiresAt,
            SameSite = SameSiteMode.None,
            Secure = false
        });
        return Ok(ApiResponse<object>.Ok(new { Token = result.Token }));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult Login(LoginRequestDto loginDto)
    {
        var result = _authService.Login(loginDto);
        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = result.RefreshTokenExpiresAt,
            SameSite = SameSiteMode.None,
            Secure = true
        });

        return Ok(ApiResponse<object>.Ok(new { Token = result.Token }));
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(ApiResponse<string>.Fail("Refresh token is missing"));
        }
        _authService.Logout(token);
        Response.Cookies.Delete("refreshToken");
        return Ok();
    }

    [HttpPost("refreshToken")]
    [AllowAnonymous]
    public ActionResult RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(ApiResponse<string>.Fail("Refresh token is missing"));
        }

        var result = _authService.RefreshToken(refreshToken);
        if (result == null)
        {
            return Unauthorized(ApiResponse<string>.Fail("Invalid or expired refresh token"));
        }

        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = result.RefreshTokenExpiresAt,
            SameSite = SameSiteMode.None,
            Secure = true
        });

        return Ok(ApiResponse<object>.Ok(new { Token = result.Token }));
    }


    [HttpPost("ping")]
    public ActionResult Ping()
    {
        return Ok(ApiResponse<string>.Ok("Pong"));
    }

}