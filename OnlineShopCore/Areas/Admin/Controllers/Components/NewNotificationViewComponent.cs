using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlineShopCore.Areas.Admin.Components
{
    public class NewNotificationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}