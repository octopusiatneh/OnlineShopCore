using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Utilities.Dtos;
using System.Collections.Generic;

namespace OnlineShopCore.Models.ProductViewModels
{
    public class ShoppingViewModel
    {
        public List<ProductCategoryViewModel> ProductCategory { get; set; }
        public PagedResult<ProductViewModel> Product { get; set; }

        public string SortBy { get; set; }

        public string SortPrice { get; set; }

        public int? PageSize { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Text="All"},
            new SelectListItem(){Text="Lastest"},
            new SelectListItem(){Text="Price:Low to High"},
            new SelectListItem(){Text="Price:High to Low"}
        };

        public List<SelectListItem> SortPrices { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Text="All"},
            new SelectListItem(){Text="$0.00 - $50.00"},
            new SelectListItem(){Text="$50.00 - $100.00"},
             new SelectListItem(){Text="$100.00 - $150.00"},
            new SelectListItem(){Text="$150.00 - $200.00"},
            new SelectListItem(){Text="$200.00+"}
        };
    }
}
