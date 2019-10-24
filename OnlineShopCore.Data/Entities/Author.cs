﻿using OnlineShopCore.Data.Enums;
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

        public Author(string authorName, int? parentId, int sortOrder, Status status)
        {
            AuthorName = authorName;
            SortOrder = sortOrder;
            ParentId = parentId;
            Status = status;
        }

        public Author(int id, string authorName, int? parentId, int sortOrder, Status status)
        {
            Id = id;
            AuthorName = authorName;
            SortOrder = sortOrder;
            ParentId = parentId;
            Status = status;
        }

        public string AuthorName { get; set; }

        public int SortOrder { get; set; }

        public int? ParentId { get; set; }

        public Status Status { set; get; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
