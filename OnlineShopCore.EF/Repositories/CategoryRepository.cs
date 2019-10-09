using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class CategoryRepository : EFRepository<Category, int>, ICategoryRepository
    {
        AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Category> GetByAlias(string alias)
        {
            return _context.Categories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}
