using OnlineShopCore.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Application.ViewModels.Common
{
    public class FeedbackViewModel
    {
        public int Id { set; get; }
        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(250)]
        [Required]
        public string Email { set; get; }

        [StringLength(500)]
        [Required]
        public string Message { set; get; }

        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
    }
}
