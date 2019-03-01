using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using OnlineShopCore.Extensions;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Utilities.Constants;

namespace OnlineShopCore.Areas.Admin.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        IFunctionService _functionService;
        public SidebarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var role = UserClaimsPrincipal.GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (role.Split(";").Contains(CommonConstants.AdminRole))
            {
                functions = await _functionService.GetAll();
            }
            else
            {
                //TODO get by permission
                functions = new List<FunctionViewModel>();

            }
            return View(functions);
        }
    }
}
