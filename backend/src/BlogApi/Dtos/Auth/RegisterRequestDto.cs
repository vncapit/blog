
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos.Auth;

public class RegisterRequestDto
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    public string Email { get; set; } = string.Empty;
}