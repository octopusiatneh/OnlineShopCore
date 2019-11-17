using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Models;
using System;
using System.Diagnostics;

namespace OnlineShopCore.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        private ISlideService _slideService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IProductService productService, ISlideService slideService,
            IStringLocalizer<HomeController> localizer)
        {
            _productService = productService;
            _slideService = slideService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            var title = _localizer["Title"];
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var homeVm = new HomeViewModel();
            homeVm.Slides = _slideService.GetSlide();
            homeVm.LastestProducts = _productService.GetLastest(16);
            homeVm.HotProducts = _productService.GetHotProduct(16);
            homeVm.TopSellProducts = _productService.GetHomeProduct(16);
            homeVm.Promotions = _productService.GetPromotion();

            return View(homeVm);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}