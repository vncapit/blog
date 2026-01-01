using System.ComponentModel.DataAnnotations;
using BlogApi.Enums;

namespace BlogApi.Dtos.Post
{
    public class PostResponseDto
    {
    public required int Id { get; set; }
    public required int AuthorId { get; set; }
    public required string AuthorName { get; set; }
    public required int CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required PostStatus Status { get; set; } = PostStatus.Draft;
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Content { get; set; }
    public required string Excerpt { get; set; } = string.Empty;
    public required string FeaturedImageUrl { get; set; } = string.Empty;
    public required string Tags { get; set; } = string.Empty;
    public required string CreatedAt { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    public required string UpdatedAt { get; set; }  = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}