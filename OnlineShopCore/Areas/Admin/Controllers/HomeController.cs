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
        private readonly IUserReportService _userReportService;
        private readonly IOrderReportService _orderReportService;
        private readonly ITopProductReportService _topProductReportService;
        public HomeController(IReportService reportService, ITopProductReportService topProductReportService,
            IUserReportService userReportService, IOrderReportService orderReportService)
        {
            _reportService = reportService;
            _userReportService = userReportService;
            _orderReportService = orderReportService;
            _topProductReportService = topProductReportService;
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

        public async Task<IActionResult> GetNewUser(string fromDate, string toDate)
        {
            return new OkObjectResult(await _userReportService.GetReport(fromDate, toDate));
        }

        public async Task<IActionResult> GetNewOrder(string fromDate, string toDate)
        {
            return new OkObjectResult(await _orderReportService.GetReport(fromDate, toDate));
        }

        public async Task<IActionResult> GetTopVisitProduct()
        {
            return new OkObjectResult(await _topProductReportService.GetReport());
        }
    }
}