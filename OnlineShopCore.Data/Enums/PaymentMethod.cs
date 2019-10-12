using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace OnlineShopCore.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Trả tiền khi nhận hàng")]
        CashOnDelivery,
        
        [Description("Paypal")]
        Paypal
    }
}
