using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.IRepositories
{
    public interface IAnnouncementService
    {
        bool MarkAsRead(string id);
    }
}
