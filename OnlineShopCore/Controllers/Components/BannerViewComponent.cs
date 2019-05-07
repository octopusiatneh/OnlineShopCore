using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlineShopCore.Views.Home
{
    public class BannerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}