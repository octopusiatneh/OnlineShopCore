using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Enums;
using System.Collections.Generic;

namespace OnlineShopCore.Application.ViewModels
{
    public class PublisherViewModel
    {
        public int Id { get; set; }

        public string PublisherName { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<ProductViewModel> Products { get; set; }
    }
}
