using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class Author : DomainEntity<int>
    {
        public string AuthorName { get; set; }
        public virtual ICollection<AuthorProduct> Products { get; set; }
    }
}
