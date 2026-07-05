using myshop.DataAccess.Repositories.IRepositories;
using myshop.Entities.Models;

namespace myshop.DataAccess.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
