using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Infrastructure.SharedKernel;

namespace OnlineShopCore.Data.Entities
{
    [Table("Slides")]
    public class Slide : DomainEntity<int>
    {
        public Slide(int id, string name, string description, string image , Status status, string content)
        {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            Status = status;
            Content = content;
        }
        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(250)]
        public string Description { set; get; }

        [StringLength(250)]
        [Required]
        public string Image { set; get; }

        public Status Status { set; get; }

        public string Content { set; get; }
    }
}
