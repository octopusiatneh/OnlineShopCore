using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
namespace OnlineShopCore.Data.EF.Repositories
{
    public class AuthorRepository : EFRepository<Author, int>, IAuthorRepository
    {
        AppDbContext _context;
        public AuthorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
