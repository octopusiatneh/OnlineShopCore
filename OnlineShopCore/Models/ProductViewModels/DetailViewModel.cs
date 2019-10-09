using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }

        public bool Available { get; set; }

        public List<ProductViewModel> RelatedProducts { get; set; }

        public CategoryViewModel Category { get; set; }

        public List<ProductImageViewModel> ProductImages { set; get; }

        public List<ProductViewModel> LastestProducts { get; set; }

        public List<TagViewModel> Tags { set; get; }

        public List<SelectListItem> Colors { get; set; }

        public List<SelectListItem> Sizes { get; set; }
    }
}
