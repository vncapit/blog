using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos;

public class AddCategoryRequestDto
{
    [Required]
    public required string Name { get; set; }
}