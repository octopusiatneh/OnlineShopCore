using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Infrastructure.Interfaces;

namespace OnlineShopCore.Data.IRepositories
{
    public interface ITagRepository : IRepository<Tag, string>
    {
    }
}
