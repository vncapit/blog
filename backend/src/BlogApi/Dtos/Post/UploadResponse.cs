 using System.ComponentModel.DataAnnotations;
using BlogApi.Enums;

namespace BlogApi.Dtos.Post
{
    public class UploadResponse
    {
    public required string Url { get; set; }
    }
}