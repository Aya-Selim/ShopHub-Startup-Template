using myshop.Business.DTOs;
using System.Collections.Generic;

namespace myshop.Business.Services.IServices
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto GetProductById(int id);
        void CreateProduct(ProductDto productDto);
        void UpdateProduct(ProductDto productDto);
        ProductDto DeleteProduct(int id);
    }
}
