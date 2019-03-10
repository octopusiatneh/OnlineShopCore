using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class BillRepository : EFRepository<Bill, int>, IBillRepository
    {
        public BillRepository(AppDbContext context) : base(context)
        {
        }
    }
}
