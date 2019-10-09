using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Utilities.Dtos;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IBillService
    {
        void Create(AnnouncementViewModel announcementVm, List<AnnouncementBillViewModel> announcementBillVm, BillViewModel billVm);
        void Update(BillViewModel billVm);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
           int pageIndex, int pageSize);

        List<BillViewModel> GetAll();

        BillViewModel GetDetail(int billId);


        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm);

        void DeleteDetail(int productId, int billId, int colorId, int sizeId);

        void UpdateStatus(int orderId, BillStatus status);

        List<BillDetailViewModel> GetBillDetails(int billId);

        void Save();
    }
}
