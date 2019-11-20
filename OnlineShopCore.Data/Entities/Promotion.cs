using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class Promotion : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Promotion()
        {

        }

        public Promotion(int id, string promotionName, DateTime dateEnd, Status status)
        {
            Id = id;
            PromotionName = promotionName;
            DateEnd = dateEnd;
            Status = status;
        }

        public Promotion(string promotionName, DateTime dateEnd, Status status)
        {
            PromotionName = promotionName;
            DateEnd = dateEnd;
            Status = status;
        }

        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        public virtual ICollection<PromotionDetail> PromotionDetails { set; get; }

        [Required]
        public string PromotionName { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; } = Status.Active;
    }
}
