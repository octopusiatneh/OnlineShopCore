using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Application.ViewModels.System
{
    public class AnnouncementBillViewModel
    {
        public int Id { set; get; }

        [StringLength(128)]
        [Required]
        public string AnnouncementId { get; set; }

        public int BillId { get; set; }

        public bool? HasRead { get; set; }
    }
}
