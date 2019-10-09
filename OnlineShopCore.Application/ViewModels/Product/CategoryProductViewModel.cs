using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Product
{
    public class CategoryProductViewModel
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public ProductViewModel Product { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
