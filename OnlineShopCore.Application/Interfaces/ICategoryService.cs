using OnlineShopCore.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Interfaces
{
    public interface ICategoryService
    {
        CategoryViewModel Add(CategoryViewModel productCategoryVm);

        void Update(CategoryViewModel productCategoryVm);

        void Delete(int id);

        List<CategoryViewModel> GetAll();

       
        CategoryViewModel GetById(int id);
      
        void Save();
    }
}