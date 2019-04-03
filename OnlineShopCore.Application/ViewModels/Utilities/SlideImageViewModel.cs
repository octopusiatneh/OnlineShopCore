﻿using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Utilities
{
    public class SlideImageViewModel
    {
        public int Id { get; set; }

        public int SlideId { get; set; }

        public virtual Slide Slide { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
    }
}
