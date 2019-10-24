using OnlineShopCore.Data.Enums;
using OnlineShopCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("Slides")]
    public class Slide : DomainEntity<int>
    {
        public Slide(int id, string image, Status status)
        {
            Id = id;
            Image = image;
            Status = status;
        }

        [StringLength(250)]
        [Required]
        public string Image { set; get; }

        public Status Status { set; get; }

    }
}
