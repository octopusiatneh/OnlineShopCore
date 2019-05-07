using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Extensions;
using OnlineShopCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Areas.Admin.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;

        public SidebarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var role = UserClaimsPrincipal.GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (role.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO get by permission
                functions = await _functionService.GetAll(string.Empty);
            }
            return View(functions);
        }
    }
}