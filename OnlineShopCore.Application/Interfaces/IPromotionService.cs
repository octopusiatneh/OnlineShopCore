using OnlineShopCore.Application.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IPromotionService
    {
        void Create(PromotionViewModel promoVm);

        void Update(PromotionViewModel promoVm);

        void UpdateStatus(int promotionId);

        List<PromotionViewModel> GetAll();

        PromotionViewModel GetDetail(int promotionId);

        PromotionDetailViewModel CreateDetail(PromotionDetailViewModel promoDetailVm);

        void DeleteDetail(int productId, int promotionId);

        List<PromotionDetailViewModel> GetPromotionDetail(int promotionId);

        void Save();
    }
}
