using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Models.ManageViewModels
{
    public class OrderHistoryViewModel
    {
        public string ProductName { get; set; }

        public int BillId { get; set; }

        public BillStatus Billstatus { get; set; }

        public string Image { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
