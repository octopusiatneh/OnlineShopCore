using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class CategoryProduct : DomainEntity<int>
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
