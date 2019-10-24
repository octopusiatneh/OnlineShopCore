using OnlineShopCore.Application.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopCore.Application.Dapper.Intefaces
{
    public interface IOrderReportService
    {
        Task<IEnumerable<NewOrderReportViewModel>> GetReport(string fromDate, string toDate);
    }
}
