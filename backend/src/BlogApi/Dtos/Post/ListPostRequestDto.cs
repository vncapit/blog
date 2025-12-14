using BlogApi.Enums;

namespace BlogApi.Dtos.Post;

public class ListPostRequestDto
{
    public string? TitleContains { get; set; }
    public int? CategoryId { get; set; }
    public PostStatus? Status { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}