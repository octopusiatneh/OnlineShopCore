using OnlineShopCore.Data.Entities;
using OnlineShopCore.Infrastructure.Interfaces;

namespace OnlineShopCore.Data.IRepositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
