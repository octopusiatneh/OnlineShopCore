using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("ProductCategories")]
    public class ProductCategory : DomainEntity<int>, ISwitchable, ISortable, IDateTracking
    {
        public ProductCategory()
        {
            Products = new List<Product>();
        }

        public ProductCategory(string name, int sortOrder, Status status, string seoAlias)
        {
            Name = name;
            SortOrder = sortOrder;
            Status = status;
            SeoAlias = seoAlias;
        }
        public string Name { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
        public string SeoAlias { set; get; }
        public virtual ICollection<Product> Products { set; get; }
    }
}