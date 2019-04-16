using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using OnlineShopCore.Infrastructure.SharedKernel;

namespace OnlineShopCore.Data.Entities
{
    [Table("AnnouncementBills")]
    public class AnnouncementBill : DomainEntity<int>
    {
        public AnnouncementBill(string announcementId, bool? hasRead)
        {
            AnnouncementId = announcementId;
            HasRead = hasRead;
        }

        [Required]
        public string AnnouncementId { get; set; }

        public bool? HasRead { get; set; }

        [ForeignKey("AnnouncementId")]
        public virtual Announcement Announcement { get; set; }
    }
    
}
