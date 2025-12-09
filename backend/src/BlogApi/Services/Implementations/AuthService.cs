
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
    private readonly TokenService _tokenService;
    public AuthService(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    public AuthResponseDto Register(RegisterRequestDto dto)
    {
        if (_context.Users.Where(u => u.Username == dto.Username).ToList().Count != 0) throw new RequestException(HttpStatusCode.BadRequest, "Username already existed");
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var user = new User { Username = dto.Username, Email = dto.Email, Role = Enums.UserRole.Author, PasswordHash = HashHelper.Hash(dto.Password) };
            _context.Users.Add(user);
            _context.SaveChanges();
            var accessToken = _tokenService.GenerateJwtToken(user);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = _tokenService.GenerateRefreshToken(),
                ExpiresAt = _tokenService.GetRefreshTokenExpiration(),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
            transaction.Commit();

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new RequestException(HttpStatusCode.InternalServerError, "An error occurred during registration", ex.Message);
        }
    }

    public AuthResponseDto Login(LoginRequestDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
        if (user == null || !HashHelper.Verify(dto.Password, user.PasswordHash))
        {
            throw new RequestException(HttpStatusCode.Unauthorized, "Invalid username or password");
        }

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = _tokenService.GetRefreshTokenExpiration(),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        var response = new AuthResponseDto
        {
            Token = _tokenService.GenerateJwtToken(user),
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
        return response;
    }

    public AuthResponseDto? RefreshToken(string token)
    {
        var refreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == token);
        if (refreshToken == null)
        {
            return null;
        }
        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            _context.RefreshTokens.Remove(refreshToken);
            _context.SaveChanges();
            return null;
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);
        if (user == null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            _context.SaveChanges();
            return null;
        }

        refreshToken.Token = _tokenService.GenerateRefreshToken();
        refreshToken.ExpiresAt = _tokenService.GetRefreshTokenExpiration();
        _context.RefreshTokens.Update(refreshToken);
        _context.SaveChanges();

        return new AuthResponseDto
        {
            Token = _tokenService.GenerateJwtToken(user),
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }

    public void Logout(string token)
    {
        var refreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == token);
        if (refreshToken != null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            _context.SaveChanges();
        }
    }
}