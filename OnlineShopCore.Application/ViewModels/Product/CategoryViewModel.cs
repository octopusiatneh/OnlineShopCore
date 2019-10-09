using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Product
{
  public  class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
        public string SeoAlias { set; get; }
        public virtual ICollection<CategoryProductViewModel> CategoryProducts { set; get; }
    }
}
