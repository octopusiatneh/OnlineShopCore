using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;


namespace OnlineShopCore.Data.EF.Repositories
{
    public class SizeRepository : EFRepository<Size, int>, ISizeRepository
    {
        public SizeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
