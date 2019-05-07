using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Extensions;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email");

            return View();
        }
    }
}