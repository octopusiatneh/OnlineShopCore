using OnlineShopCore.Application.ViewModels.Product;

namespace OnlineShopCore.Models
{
    public class ShoppingCartViewModel
    {
        public ProductViewModel Product { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }
    }
}
