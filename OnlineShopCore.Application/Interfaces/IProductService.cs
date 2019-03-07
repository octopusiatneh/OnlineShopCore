using OnlineShopCore.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void Save();
    }
}
