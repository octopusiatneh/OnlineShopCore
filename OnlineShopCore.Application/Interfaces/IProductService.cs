using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        ProductViewModel Add(ProductViewModel product);

        void AddImages(int productId, string[] images);

        void Delete(int id);

        PagedResult<ProductViewModel> Filter(string filter, int page, int pageSize);

        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<ProductViewModel> GetAvailableProductForPromotion(DateTime dateStart);

        ProductViewModel GetById(int id);

        List<ProductViewModel> GetByName(string keyword);

        List<ProductViewModel> GetHomeProduct(int top);

        List<ProductViewModel> GetHotProduct(int top);

        List<ProductImageViewModel> GetImages(int productId);

        List<ProductViewModel> GetLastest(int top);

        List<PromotionViewModel> GetPromotion();

        List<ProductViewModel> GetRelatedProducts(int id, int top);

        void ImportExcel(string filePath, int categoryId,int authorId, int publisherId);

        void IncreaseViewCount(int id);

        void Save();

        void Update(ProductViewModel product);

    }
}
