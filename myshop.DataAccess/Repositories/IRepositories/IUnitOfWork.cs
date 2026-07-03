using myshop.DataAccess.Repositories.IRepositories;
using System;

namespace myshop.DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        int Save();
    }
}
