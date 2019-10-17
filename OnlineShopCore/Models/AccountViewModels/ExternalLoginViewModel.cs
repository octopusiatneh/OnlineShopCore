using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]

        public string FullName { get; set; }

        [Required]

        public string DoB { get; set; }

        [Required]

        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

    }
}
