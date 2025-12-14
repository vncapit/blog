using System;
using System.Security.Claims;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Services.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            return Convert.ToInt32(userId);
        }

        public string? GetUserName()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            return claimsPrincipal?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}