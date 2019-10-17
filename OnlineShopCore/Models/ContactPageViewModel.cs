using OnlineShopCore.Application.ViewModels;
using OnlineShopCore.Application.ViewModels.Common;

namespace OnlineShopCore.Models
{
    public class ContactPageViewModel
    {
        public ContactViewModel Contact { set; get; }

        public FeedbackViewModel Feedback { set; get; }
    }
}
