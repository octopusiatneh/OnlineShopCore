using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopCore.Application.ViewModels.Utilities
{
    public class PromotionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string PromotionName { get; set; }

        public DateTime DateModified { get; set; }

        [Required]

        public DateTime DateExpired { get; set; }

        public Status Status { get; set; }

        public List<PromotionDetailViewModel> PromotionDetails { set; get; }

    }
}
