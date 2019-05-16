using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Extensions;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    [Authorize(Roles ="Staff,Admin")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email");

            return View();
        }
    }
}