﻿using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopCore.Models
{
    public class CheckoutViewModel : BillViewModel
    {
        public List<ShoppingCartViewModel> Carts { get; set; }

        public List<EnumModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                    .Select(c => new EnumModel
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();
            }
        }

        public List<EnumModel> ListBillStatus
        {
            get
            {
                return ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                    .Select(c => new EnumModel
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();
            }
        }
    }
}
