using OnlineShopCore.Data.Enums;

namespace OnlineShopCore.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
