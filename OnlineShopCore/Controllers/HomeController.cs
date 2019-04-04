using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Models;

namespace OnlineShopCore.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        private ISlideService _slideService;
       
        public HomeController(IProductService productService, ISlideService slideService)
        {    
            _productService = productService;
            _slideService = slideService;
        }

        public IActionResult Index()
        {    
            var homeVm = new HomeViewModel();
            homeVm.Slides = _slideService.GetSlide();
            homeVm.LastestProducts = _productService.GetLastest(16);
            homeVm.HotProducts = _productService.GetHotProduct(16);
            homeVm.TopSellProducts = _productService.GetHomeProduct(16);

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
    }
}
