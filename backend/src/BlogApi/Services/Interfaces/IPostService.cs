namespace BlogApi.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BlogApi.Models;
    using BlogApi.Dtos.Post;
    using BlogApi.Dtos.Common;

    public interface IPostService
    {
        Task<PaginationList<Post>> ListPostsAsync(ListPostRequestDto dto);
        Task<Post> GetPostByIdAsync(int id);
        Task<int> CreatePostAsync(AddPostRequestDto dto);
        Task<int> UpdatePostAsync(UpdatePostRequestDto dto);
        Task<int> DeletePostByIdAsync(int id);
    }
}