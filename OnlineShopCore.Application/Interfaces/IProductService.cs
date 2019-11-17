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
        List<ProductViewModel> GetAll();
        List<ProductViewModel> GetAllWithNoPromotionPrice();
        PagedResult<ProductViewModel> Filter(string filter, int page, int pageSize);
        List<ProductViewModel> GetByName(string keyword);
        ProductViewModel Add(ProductViewModel product);
        void Update(ProductViewModel product);
        void IncreaseViewCount(int id);
        void Delete(int id);
        ProductViewModel GetById(int id);
        void ImportExcel(string filePath, int categoryId,int authorId, int publisherId);
        void Save();
        void AddImages(int productId, string[] images);
        List<ProductImageViewModel> GetImages(int productId);
        PagedResult<ProductViewModel> GetAllPaging(string keyword, int page, int pageSize);
        List<ProductViewModel> GetLastest(int top);
        List<ProductViewModel> GetHotProduct(int top);
        List<ProductViewModel> GetHomeProduct(int top);
        List<PromotionViewModel> GetPromotion();
        List<ProductViewModel> GetRelatedProducts(int id, int top);

    }
}
