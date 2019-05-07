using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace OnlineShopCore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorCode = "404";
                    ViewBag.ErrorMessage = "PAGE NOT FOUND !";
                    ViewBag.Message = "Sorry the page you requested could not be found"; ;
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;

                case 500:
                    ViewBag.ErrorCode = "500";
                    ViewBag.ErrorMessage = "SERVER DOWN !";
                    ViewBag.Message = "Sorry something went wrong on the server";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
            }
            return View();
        }
    }
}