using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Application.Dapper.Intefaces;
using OnlineShopCore.Extensions;
using System.Threading.Tasks;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Staff,Admin")]
    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;

        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email");

            return View();
        }

        public async Task<IActionResult> GetRevenue(string fromDate, string toDate)
        {
            return new OkObjectResult(await _reportService.GetReport(fromDate, toDate));
        }
    }
}