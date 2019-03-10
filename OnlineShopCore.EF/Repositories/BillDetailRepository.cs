using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class BillDetailRepository : EFRepository<BillDetail, int>, IBillDetailRepository
    {
        public BillDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
