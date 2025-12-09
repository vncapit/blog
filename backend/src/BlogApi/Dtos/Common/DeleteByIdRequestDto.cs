using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos.Common;

public class DeleteByIdRequestDto
{
    [Required]
    public int Id { get; set; }
}