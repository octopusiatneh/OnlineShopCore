using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class SlideRepository : EFRepository<Slide, int>, ISlideRepository
    {
        public SlideRepository(AppDbContext context) : base(context)
        {
        }
    }
}
