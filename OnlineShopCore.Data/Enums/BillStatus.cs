using System.ComponentModel;

namespace OnlineShopCore.Data.Enums
{
    public enum BillStatus
    {
        [Description("Chưa duyệt")]
        New,
        [Description("Đã thanh toán")]
        Paid,  
        [Description("Đã chuyển cho bên vận chuyển")]
        InDelivery,
        [Description("Đơn hàng bị trả")]
        Returned,
        [Description("Đơn hàng bị hủy")]
        Cancelled,
        [Description("Đã giao hàng")]
        Completed     
    }
}
