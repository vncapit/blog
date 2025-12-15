
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using BlogApi.CustomExceptions;
using BlogApi.Data;
using BlogApi.Dtos.Common;
using BlogApi.Dtos.Post;
using BlogApi.Enums;
using BlogApi.Models;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

public class PostService : IPostService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly AppDbContext _dbContext;
    public PostService(ICurrentUserService currentUserService, AppDbContext dbContext)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;
    }
    public async Task<int> CreatePostAsync(AddPostRequestDto dto)
    {
        var post = new Post
        {
            Title = dto.Title,
            Slug = GenerateSlug(dto.Title),
            Content = dto.Content,
            CategoryId = dto.CategoryId,
            Status = dto.Status,
            AuthorId = _currentUserService.GetUserId() ?? 0
        };
        _dbContext.Posts.Add(post);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeletePostByIdAsync(int id)
    {
        var post = _dbContext.Posts.Find(id);
        if (post != null)
        {
            post.Status = PostStatus.Deleted;
            post.UpdatedAt = DateTime.UtcNow;
            return await _dbContext.SaveChangesAsync();
        }
        throw new RequestException(System.Net.HttpStatusCode.BadGateway, "Post not found");
    }

    public async Task<PaginationList<Post>> ListPostsAsync(ListPostRequestDto dto)
    {
        var query = _dbContext.Posts.AsQueryable();
        if (dto.TitleContains != null)
        {
            query = query.Where(p => p.Title.ToLower().Contains(dto.TitleContains.ToLower()));
        }
        if (dto.CategoryId != null)
        {
            query = query.Where(p => p.CategoryId == dto.CategoryId);
        }
        if (dto.Status != null)
        {
            query = query.Where(p => p.Status == dto.Status);
        }
        var res = new PaginationList<Post>();
        res.TotalCount = query.Count();
        res.Items = await query.Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync();
        return res;
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        var post = await _dbContext.Posts.FindAsync(id);
        if (post == null)
        {
            throw new RequestException(System.Net.HttpStatusCode.BadGateway, "Post not found");
        }
        return post;

    }

    public async Task<int> UpdatePostAsync(UpdatePostRequestDto dto)
    {
        var post = await _dbContext.Posts.FindAsync(dto.Id);
        if (post == null)
        {
            throw new RequestException(System.Net.HttpStatusCode.BadGateway, "Post not found");
        }
        post.Title = dto.Title;
        post.Slug = GenerateSlug(dto.Title);
        post.Content = dto.Content;
        post.CategoryId = dto.CategoryId;
        post.Status = dto.Status;
        post.UpdatedAt = DateTime.UtcNow;
        _dbContext.Posts.Update(post);
        return _dbContext.SaveChanges();
    }


    private string GenerateSlug(string text)
    {
        text = RemoveDiacritics(text).ToLowerInvariant();

        // replace invalid chars
        text = Regex.Replace(text, @"[^a-z0-9\s-]", "-");

        // replace multiple spaces with single dash
        text = Regex.Replace(text, @"\s+", "-").Trim('-');

        // remove multiple dashes
        text = Regex.Replace(text, "-+", "-");

        return text;
    }

    public static string RemoveDiacritics(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var manuallyReplaced = text.Replace('đ', 'd').Replace('Đ', 'D');

        var normalized = manuallyReplaced.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var ch in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}