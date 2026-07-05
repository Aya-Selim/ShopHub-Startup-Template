using AutoMapper;
using myshop.Business.DTOs;
using myshop.Business.Services.IServices;
using myshop.DataAccess.Repositories.IRepositories;
using myshop.Entities.Models;
using System.Collections.Generic;

namespace myshop.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            var categories = _unitOfWork.Category.GetAll();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public CategoryDto GetCategoryById(int id)
        {
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            return _mapper.Map<CategoryDto>(category);
        }

        public void CreateCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();
        }

        public void UpdateCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
        }

        public void DeleteCategory(int id)
        {
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category != null)
            {
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
            }
        }
    }
}
