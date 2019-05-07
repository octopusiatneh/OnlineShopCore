using Microsoft.AspNetCore.Mvc;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult NewNotification()
        {
            return ViewComponent("NewNotification");
        }
    }
}