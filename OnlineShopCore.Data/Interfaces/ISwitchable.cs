using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
