﻿using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Utilities
{
    public class SlideViewModel
    {
        public int Id { get; set; }

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