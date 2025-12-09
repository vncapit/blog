
using BlogApi.Enums;

namespace BlogApi.Models;

public class Post
{
    public int Id { get; set; }
    public required int AuthorId { get; set; }
    public required int CategoryId { get; set; }
    public required PostStatus Status { get; set; } = PostStatus.Draft;
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}