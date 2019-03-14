using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Product
{
    public class ShopViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public List<ProductCategoryViewModel> ProductCategory { get; set; }
    }
}
