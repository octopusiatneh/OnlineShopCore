using OnlineShopCore.Application.ViewModels.Product;
using System.Collections.Generic;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm);

        void Update(ProductCategoryViewModel productCategoryVm);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();

        List<ProductCategoryViewModel> GetAll(string keyword);

        ProductCategoryViewModel GetById(int id);

        void Save();
    }
}