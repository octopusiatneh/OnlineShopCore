using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Extensions;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {[Area("Admin")]
        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email");

            return View();
        }
    }
}