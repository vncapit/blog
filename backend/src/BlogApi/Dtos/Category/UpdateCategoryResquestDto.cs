using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos;

public class UpdateCategoryRequestDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
}