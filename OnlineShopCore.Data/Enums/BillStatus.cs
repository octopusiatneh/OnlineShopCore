using System.ComponentModel;

namespace OnlineShopCore.Data.Enums
{
    public enum BillStatus
    {
        [Description("Chưa duyệt")]
        New,
        [Description("Đã chuyển cho bên vận chuyển")]
        InProgress,
        [Description("Đơn hàng bị trả")]
        Returned,
        [Description("Đơn hàng bị hủy")]
        Cancelled,
        [Description("Đã giao hàng")]
        Completed
    }
}
