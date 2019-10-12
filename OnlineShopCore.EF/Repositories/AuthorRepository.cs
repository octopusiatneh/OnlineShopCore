using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
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
