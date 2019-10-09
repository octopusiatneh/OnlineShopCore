using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class AuthorProduct : DomainEntity<int>
    {
        public int ProductId { get; set; }

        public int AuthorId { get; set; }

        public Product Product { get; set; }

        public Author Author { get; set; }
    }
}
