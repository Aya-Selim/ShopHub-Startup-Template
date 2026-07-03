using myshop.Entities.Models;

namespace myshop.DataAccess.Repositories.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
