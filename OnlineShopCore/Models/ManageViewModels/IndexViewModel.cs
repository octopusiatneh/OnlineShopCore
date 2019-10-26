using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string StatusMessage { get; set; }
    }
}
