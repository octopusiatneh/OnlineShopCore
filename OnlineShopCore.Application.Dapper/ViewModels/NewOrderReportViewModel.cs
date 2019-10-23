using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Dapper.ViewModels
{
    public class NewOrderReportViewModel
    {
        public DateTime Date { get; set; }

        public decimal TotalNewOrder { get; set; }
    }
}
