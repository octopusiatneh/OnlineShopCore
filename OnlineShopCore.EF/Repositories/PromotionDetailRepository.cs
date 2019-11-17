using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class PromotionDetailRepository : EFRepository<PromotionDetail, int>, IPromotionDetailRepository
    {
        public PromotionDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
