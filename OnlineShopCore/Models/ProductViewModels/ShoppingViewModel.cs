using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Models.ProductViewModels
{
    public class ShoppingViewModel
    {
        public List<ProductCategoryViewModel> ProductCategory { get; set; }
        public PagedResult<ProductViewModel> Product { get; set; }

        public string SortType { get; set; }

        public int? PageSize { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value="default",Text="Default"},
            new SelectListItem(){Value="lastest",Text="Lastest"},
            new SelectListItem(){Value="pricelth",Text="Price:Low to High"},
            new SelectListItem(){Value="pricehtl",Text="Price:High to Low"}
        };
    }
}
