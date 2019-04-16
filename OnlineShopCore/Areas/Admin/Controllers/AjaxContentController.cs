using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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