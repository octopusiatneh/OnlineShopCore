using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace OnlineShopCore.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Cash on delivery")]
        CashOnDelivery,
        
        [Description("ATM")]
        ATM
    }
}
