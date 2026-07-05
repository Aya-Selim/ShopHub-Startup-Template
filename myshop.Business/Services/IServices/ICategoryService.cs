using myshop.Business.DTOs;
using System.Collections.Generic;

namespace myshop.Business.Services.IServices
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAllCategories();
        CategoryDto GetCategoryById(int id);
        void CreateCategory(CategoryDto categoryDto);
        void UpdateCategory(CategoryDto categoryDto);
        void DeleteCategory(int id);
    }
}
