using System.ComponentModel.DataAnnotations;
using BlogApi.Enums;

namespace BlogApi.Dtos.Post
{
    public class AddPostRequestDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Draft;

    }
}