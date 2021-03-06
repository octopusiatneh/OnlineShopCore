﻿using OnlineShopCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("SlideImages")]
    public class SlideImage : DomainEntity<int>
    {
        public int SlideId { get; set; }

        [ForeignKey("SlideId")]
        public virtual Slide Slide { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
    }
}
