using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class Publisher : DomainEntity<int>
    {
        public int NamePublisher { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
