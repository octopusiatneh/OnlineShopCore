using OnlineShopCore.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Utilities
{
    public class PromotionDetailViewModel
    {
        public int Id { get; set; }

        public int PromotionId { set; get; }

        public int ProductId { set; get; }

        [Range(0, 100, ErrorMessage = "Enter number between 0 to 100")]
        public int PromotionPercent { get; set; }

        public PromotionViewModel Promotion { set; get; }

        public ProductViewModel Product { set; get; }
    }
}
