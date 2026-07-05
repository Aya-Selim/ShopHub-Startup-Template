using myshop.Entities.Models;

namespace myshop.DataAccess.Repositories.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
