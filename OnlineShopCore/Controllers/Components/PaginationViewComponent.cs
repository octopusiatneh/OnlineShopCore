﻿using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Utilities.Dtos;
using System.Threading.Tasks;

namespace OnlineShopCore.Controllers.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}