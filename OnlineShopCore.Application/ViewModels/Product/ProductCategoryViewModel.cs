using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;

namespace OnlineShopCore.Application.ViewModels.Product
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
        public string SeoAlias { set; get; }

        public ICollection<ProductViewModel> Products { set; get; }
    }
}
