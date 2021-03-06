﻿using OnlineShopCore.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Application.ViewModels.Utilities
{
    public class SlideViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Image { set; get; }

        public Status Status { set; get; }

    }
}
