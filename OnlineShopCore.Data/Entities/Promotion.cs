using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class Promotion : DomainEntity<int>
    {
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public  double PercentOfPromotion { get; set; }
    }
}
