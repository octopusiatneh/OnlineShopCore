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
