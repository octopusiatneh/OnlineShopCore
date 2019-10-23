using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Dapper.ViewModels
{
    public class NewUserReportViewModel
    {
        public DateTime Date { get; set; }

        public decimal TotalNewUser { get; set; }
    }
}
