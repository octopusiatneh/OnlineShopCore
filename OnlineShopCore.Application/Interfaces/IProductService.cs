using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> Filter(string filter,int page,int pageSize);

        List<ProductViewModel> GetByName(string keyword);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void ImportExcel(string filePath, int categoryId);
        void Save();



        void AddImages(int productId, string[] images);

        List<ProductImageViewModel> GetImages(int productId);


        PagedResult<ProductViewModel> GetAllPaging(string keyword, int page, int pageSize);
        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetHotProduct(int top);
        List<ProductViewModel> GetHomeProduct(int top);


        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<TagViewModel> GetProductTags(int productId);


    }
}
