using myshop.DataAccess.Repositories.IRepositories;
using myshop.Entities.Models;
using System.Linq;

namespace myshop.DataAccess.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Update(Product product)
        {
            var objFromDb = _context.Products.FirstOrDefault(s => s.Id == product.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = product.Name;
                objFromDb.Description = product.Description;
                objFromDb.Price = product.Price;
                objFromDb.CategoryId = product.CategoryId;
                if (product.Img != null)
                {
                    objFromDb.Img = product.Img;
                }
            }
        }
    }
}
