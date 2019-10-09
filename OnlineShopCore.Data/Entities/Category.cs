using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using OnlineShopCore.Infrastructure.SharedKernel;

namespace OnlineShopCore.Data.Entities
{
    [Table("Categories")]
    public class Category : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Category()
        {

        }

        public Category(string name,Status status,string seoAlias)
        {
            Name = name;      
            Status = status;
            SeoAlias = seoAlias;
        }
        public string Name { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }    
        public Status Status { set; get; }  
        public string SeoAlias { set; get; }

        public virtual ICollection<CategoryProduct> Products { set; get; }
    }
}