using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlineShopCore.Controllers.Components
{
    public class BlogsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}