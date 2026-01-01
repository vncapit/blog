
using BlogApi.Dtos;
using BlogApi.Dtos.Common;
using BlogApi.Dtos.Post;
using BlogApi.Models;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("list")]
    public async Task<ActionResult> ListPosts([FromQuery] ListPostRequestDto dto)
    {
        return Ok(ApiResponse<PaginationList<PostResponseDto>>.Ok(await _postService.ListPostsAsync(dto)));
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddPost(AddPostRequestDto dto)
    {
        return Ok(ApiResponse<int>.Ok(await _postService.CreatePostAsync(dto)));
    }

    [HttpPut("update")]
    public async Task<ActionResult> UpdatePost(UpdatePostRequestDto dto)
    {
        return Ok(ApiResponse<int>.Ok(await _postService.UpdatePostAsync(dto)));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeletePost(DeleteByIdRequestDto dto)
    {
        return Ok(ApiResponse<int>.Ok(await _postService.DeletePostByIdAsync(dto.Id)));
    }
}