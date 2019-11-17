using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class PromotionDetail : DomainEntity<int>
    {
        public PromotionDetail()
        {

        }

        public PromotionDetail(int id, int promotionId, int productId, int promotionPercent, decimal price)
        {
            Id = id;
            PromotionId = promotionId;
            PromotionPercent = promotionPercent;
            Price = price;
        }

        public PromotionDetail(int promotionId, int productId, int promotionPercent, decimal price)
        {
            PromotionId = promotionId;
            PromotionPercent = promotionPercent;
            Price = price;
        }
        public int PromotionId { set; get; }

        public int ProductId { set; get; }

        public int PromotionPercent { set; get; }

        public decimal Price { set; get; }

        [ForeignKey("PromotionId")]
        public virtual Promotion Promotion { set; get; }
            
        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }
    }
}
