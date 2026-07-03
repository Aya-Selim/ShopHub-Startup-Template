using AutoMapper;
using myshop.Business.DTOs;
using myshop.Business.Services.IServices;
using myshop.DataAccess.Repositories.IRepositories;
using myshop.Entities.Models;
using System.Collections.Generic;

namespace myshop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ProductDto> GetAllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public ProductDto GetProductById(int id)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Category");
            return _mapper.Map<ProductDto>(product);
        }

        public void CreateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _unitOfWork.Product.Add(product);
            _unitOfWork.Save();
        }

        public void UpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
        }

        public ProductDto DeleteProduct(int id)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == id);
            if (product != null)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return productDto;
            }
            return null;
        }
    }
}
