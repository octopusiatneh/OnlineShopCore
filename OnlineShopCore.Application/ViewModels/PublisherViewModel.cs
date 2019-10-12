using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.ViewModels
{
    public class PublisherViewModel
    {
        public int Id { get; set; }

        public string NamePublisher { get; set; }
        public int? ParentId { get; set; }

        public int SortOrder { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<ProductViewModel> Products { get; set; }
    }
}
