using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class ColorRepository : EFRepository<Color, int>, IColorRepository
    {
        public ColorRepository(AppDbContext context) : base(context)
        {
        }
    }
}
