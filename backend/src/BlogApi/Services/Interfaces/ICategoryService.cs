using BlogApi.Dtos;
using BlogApi.Dtos.Common;
using BlogApi.Models;

namespace BlogApi.Services.Interfaces;

public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync();
    public Task<IEnumerable<Category>> AddCategoryAsync(AddCategoryRequestDto dto);
    public Task<IEnumerable<Category>> UpdateCategoryAsync(UpdateCategoryRequestDto dto);
    public Task<IEnumerable<Category>> DeleteCategoryByIdAsync(int id);
}