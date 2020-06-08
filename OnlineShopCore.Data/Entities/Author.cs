using OnlineShopCore.Data.Enums;
using OnlineShopCore.Infrastructure.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("Authors")]
    public class Author : DomainEntity<int>
    {
        public Author()
        {

        }

        public Author(string authorName, Status status)
        {
            AuthorName = authorName;
            Status = status;
        }

        public Author(int id, string authorName, Status status)
        {
            Id = id;
            AuthorName = authorName;
            Status = status;
        }

        public string AuthorName { get; set; }

        public Status Status { set; get; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
