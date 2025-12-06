
using BlogApi.Dtos.Auth;

namespace BlogApi.Services.Interfaces;

public interface IAuthService
{
    public AuthResponseDto Register(RegisterRequestDto registerRequestDto);
    public AuthResponseDto Login(LoginRequestDto loginRequestDto);
    public void Logout();
}