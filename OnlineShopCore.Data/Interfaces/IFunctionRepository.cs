using OnlineShopCore.Data.Entities;
using OnlineShopCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Interfaces
{
   public interface IFunctionRepository : IRepository<Function, string>
    {
    }
}
