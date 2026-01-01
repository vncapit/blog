using System.ComponentModel.DataAnnotations;
using BlogApi.Enums;

namespace BlogApi.Dtos.Post
{
    public class UpdatePostRequestDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Excerpt { get; set; } = string.Empty;
        [Required]
        public string FeaturedImageUrl { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public PostStatus Status { get; set; } = PostStatus.Draft;

    }
}