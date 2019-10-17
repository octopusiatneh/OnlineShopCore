using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class SlideImageRepository : EFRepository<SlideImage, int>, ISlideImageRepository
    {
        public SlideImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
