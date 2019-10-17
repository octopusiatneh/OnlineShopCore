using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Enums;
using System.Collections.Generic;

namespace OnlineShopCore.Application.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public int SortOrder { get; set; }
        public int? ParentId { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<ProductViewModel> Products { get; set; }
    }
}
