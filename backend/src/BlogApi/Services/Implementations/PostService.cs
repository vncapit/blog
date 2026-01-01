
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
            Excerpt = dto.Excerpt,
            FeaturedImageUrl = dto.FeaturedImageUrl,
            Tags = dto.Tags,
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

    public async Task<PaginationList<PostResponseDto>> ListPostsAsync(ListPostRequestDto dto)
    {
        
        var query = from p in _dbContext.Posts where p.Status != PostStatus.Deleted
        join u in _dbContext.Users on p.AuthorId equals u.Id
        join c in _dbContext.Categories on p.CategoryId equals c.Id
        select new PostResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Slug = p.Slug,
            Content = p.Content,                    
            CategoryId = p.CategoryId,
            CategoryName = c.Name,
            Excerpt = p.Excerpt,
            FeaturedImageUrl = p.FeaturedImageUrl,
            Tags = p.Tags,
            Status = p.Status,
            AuthorId = p.AuthorId,
            AuthorName = u.Username,
            CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = p.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        };

        if (dto.TitleContains != null)
        {
            query = query.Where(p => EF.Functions.Like(p.Title, $"%{dto.TitleContains}%"));
        }
        if (dto.CategoryId != null)
        {
            query = query.Where(p => p.CategoryId == dto.CategoryId);
        }
        if (dto.Status != null)
        {
            query = query.Where(p => p.Status == dto.Status);
        }
        var res = new PaginationList<PostResponseDto>();
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
        post.Excerpt = dto.Excerpt;
        post.FeaturedImageUrl = dto.FeaturedImageUrl;
        post.Tags = dto.Tags;
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