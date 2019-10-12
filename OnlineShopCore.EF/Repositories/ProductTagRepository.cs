using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.IRepositories;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class ProductTagRepository : EFRepository<ProductTag, int>, IProductTagRepository
    {
        public ProductTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
