using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AllowAnonymous]
    public class BaseController : Controller
    {
    }
}