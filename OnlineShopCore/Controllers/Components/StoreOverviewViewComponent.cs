using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlineShopCore.Controllers.Components
{
    public class StoreOverviewViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}