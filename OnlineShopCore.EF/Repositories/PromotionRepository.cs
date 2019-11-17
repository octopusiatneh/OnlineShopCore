using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class PromotionRepository : EFRepository<Promotion, int>, IPromotionRepository
    {
        public PromotionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
