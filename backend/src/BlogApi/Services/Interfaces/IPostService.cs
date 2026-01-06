namespace BlogApi.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BlogApi.Models;
    using BlogApi.Dtos.Post;
    using BlogApi.Dtos.Common;

    public interface IPostService
    {
        Task<PaginationList<PostResponseDto>> ListPostsAsync(ListPostRequestDto dto);
        Task<Post> GetPostByIdAsync(int id);
        Task<int> CreatePostAsync(AddPostRequestDto dto);
        Task<int> UpdatePostAsync(UpdatePostRequestDto dto);
        Task<int> DeletePostByIdAsync(int id);
        Task<string> UploadImageAsync(Microsoft.AspNetCore.Http.IFormFile file);
    }
}