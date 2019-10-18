using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopCore.Application.ViewModels;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using System.Collections.Generic;

namespace OnlineShopCore.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }

        public bool Available { get; set; }

        public List<ProductViewModel> RelatedProducts { get; set; }

        public ProductCategoryViewModel Category { get; set; }

        public List<ProductImageViewModel> ProductImages { set; get; }

        public List<ProductViewModel> LastestProducts { get; set; }

        public AuthorViewModel Author { get; set; }

        public PublisherViewModel Publisher { get; set; }
    }
}
