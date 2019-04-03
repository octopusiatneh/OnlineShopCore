using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class SlideImageRepository : EFRepository<SlideImage, int>, ISlideImageRepository
    {
        public SlideImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
