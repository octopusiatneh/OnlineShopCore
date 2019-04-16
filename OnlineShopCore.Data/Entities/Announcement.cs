using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using OnlineShopCore.Infrastructure.SharedKernel;

namespace OnlineShopCore.Data.Entities
{
    [Table("Announcements")]
    public class Announcement  : DomainEntity<string>,ISwitchable,IDateTracking
    {
        public Announcement()
        {
            AnnouncementBills = new List<AnnouncementBill>();
        }

        public Announcement(string title, string content, Status status)
        {
            Title = title;
            Content = content;
            Status = status;
        }

        [Required]
        [StringLength(250)]
        public string Title { set; get; }

        [StringLength(250)]
        public string Content { set; get; }

        [ForeignKey("BillId")]
        public virtual Bill Bill { get; set; }

        public virtual ICollection<AnnouncementBill> AnnouncementBills { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
    }
}
