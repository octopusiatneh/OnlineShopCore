using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.Utilities;
using System.Collections.Generic;

namespace OnlineShopCore.Models
{
    public class HomeViewModel
    {

        public IEnumerable<SlideViewModel> Slides { get; set; }
        public IEnumerable<ProductViewModel> LastestProducts { get; set; }
        public IEnumerable<ProductViewModel> HotProducts { get; set; }
        public IEnumerable<ProductViewModel> TopSellProducts { get; set; }

    }
}
