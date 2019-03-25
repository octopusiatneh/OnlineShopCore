using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class FeedbackRepository : EFRepository<Feedback, int>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}
