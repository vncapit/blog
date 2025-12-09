using BlogApi.CustomExceptions;
using BlogApi.Data;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApi.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<Category>> UpdateCategoryAsync(UpdateCategoryRequestDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.Id);
            if (category == null) throw new RequestException(System.Net.HttpStatusCode.BadRequest, "Category not found");

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());
            if (existingCategory != null && existingCategory.Id != dto.Id)
            {
                if (existingCategory != null) throw new RequestException(System.Net.HttpStatusCode.BadRequest, "Category with the same name already exists");
            }

            category.Name = dto.Name;
            await _context.SaveChangesAsync();
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<Category>> AddCategoryAsync(AddCategoryRequestDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());
            if (existingCategory != null) throw new RequestException(System.Net.HttpStatusCode.BadRequest, "Category with the same name already exists");

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<Category>> DeleteCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new RequestException(System.Net.HttpStatusCode.BadRequest, "Category not found");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }
    }
}