
using BlogApi.Dtos;
using BlogApi.Dtos.Common;
using BlogApi.Models;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<Category>> ListCategories()
    {
        return Ok(ApiResponse<IEnumerable<Category>>.Ok(await _categoryService.GetAllCategoriesAsync()));
    }

    [HttpPost("add")]
    public async Task<ActionResult<Category>> AddCategory(AddCategoryRequestDto dto)
    {
        return Ok(ApiResponse<IEnumerable<Category>>.Ok(await _categoryService.AddCategoryAsync(dto)));
    }

    [HttpPut("update")]
    public async Task<ActionResult<Category>> UpdateCategory(UpdateCategoryRequestDto dto)
    {
        return Ok(ApiResponse<IEnumerable<Category>>.Ok(await _categoryService.UpdateCategoryAsync(dto)));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<Category>> DeleteCategory(DeleteByIdRequestDto dto)
    {
        return Ok(ApiResponse<IEnumerable<Category>>.Ok(await _categoryService.DeleteCategoryByIdAsync(dto.Id)));
    }
}