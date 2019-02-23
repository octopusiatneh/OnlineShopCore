using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.Enums;

namespace OnlineShopCore.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
