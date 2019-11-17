using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.IRepositories
{
    public interface IPromotionRepository : IRepository<Promotion, int>
    {
    }
}
