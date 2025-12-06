
using BlogApi.CustomExceptions;
using BlogApi.Data;
using BlogApi.Dtos.Auth;
using BlogApi.Helpers;
using BlogApi.Models;
using BlogApi.Services.Commons;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;
    public AuthService(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwt = jwtService;
    }
    public AuthResponseDto Register(RegisterRequestDto dto)
    {
        if (_context.Users.Where(u => u.Username == dto.Username).ToList().Count != 0) throw new RequestException(HttpStatusCode.BadRequest, "Username already existed");
        var user = new User { Username = dto.Username, Email = dto.Email, Role = Enums.UserRole.Author, PasswordHash = HashHelper.Hash(dto.Password) };
        _context.Users.Add(user);
        _context.SaveChanges();
        var response = new AuthResponseDto
        {
            Token = _jwt.GenerateJwtToken(user),
        };
        return response;
    }

    public AuthResponseDto Login(LoginRequestDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
        if (user == null || !HashHelper.Verify(dto.Password, user.PasswordHash))
        {
            throw new RequestException(HttpStatusCode.Unauthorized, "Invalid username or password");
        }
        var response = new AuthResponseDto
        {
            Token = _jwt.GenerateJwtToken(user),
        };
        return response;
    }

    public void Logout()
    {
        throw new Exception();

    }
}