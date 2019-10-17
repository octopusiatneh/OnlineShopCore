using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Utilities.Dtos;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IAnnouncementService
    {
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(int pageIndex, int pageSize);
        bool MarkAsRead(string id);
    }
}
