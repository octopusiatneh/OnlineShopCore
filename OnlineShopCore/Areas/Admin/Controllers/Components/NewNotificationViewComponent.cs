using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
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
