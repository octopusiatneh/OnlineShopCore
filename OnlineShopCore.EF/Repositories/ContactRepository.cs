using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class ContactRepository : EFRepository<Contact, string>, IContactRepository
    {
        public ContactRepository(AppDbContext context) : base(context)
        {
        }
    }
}
