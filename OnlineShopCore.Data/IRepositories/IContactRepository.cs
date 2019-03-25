using OnlineShopCore.Data.Entities;
using OnlineShopCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.IRepositories
{
    public interface IContactRepository : IRepository<Contact, string>
    {
    }
}
