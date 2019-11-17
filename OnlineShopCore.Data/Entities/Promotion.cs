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

        public Promotion(int id, string promotionName, DateTime dateExpired, Status status)
        {
            Id = id;
            PromotionName = promotionName;
            DateExpired = dateExpired;
            Status = status;
        }

        public Promotion(string promotionName, DateTime dateExpired, Status status)
        {
            PromotionName = promotionName;
            DateExpired = dateExpired;
            Status = status;
        }

        [Required]
        public string PromotionName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [Required]
        public DateTime DateExpired { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; } = Status.Active;

        public virtual ICollection<PromotionDetail> PromotionDetails { set; get; }
    }
}
