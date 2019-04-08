using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopCore.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult Header()
        {
            return ViewComponent("Header");
        }

        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart");
        }

        public IActionResult HeaderMobile()
        {
            return ViewComponent("HeaderMobile");
        }
    }
}