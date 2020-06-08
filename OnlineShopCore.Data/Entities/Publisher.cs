using OnlineShopCore.Data.Enums;
using OnlineShopCore.Infrastructure.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("Publishers")]
    public class Publisher : DomainEntity<int>
    {
        public Publisher()
        {

        }

        public Publisher(string name, Status status)
        {
            PublisherName = name;

            Status = status;
        }

        public Publisher(int id, string name, Status status)
        {
            Id = id;
            PublisherName = name;
            Status = status;
        }

        public string PublisherName { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
