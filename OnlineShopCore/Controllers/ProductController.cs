using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Models.ProductViewModels;

namespace OnlineShopCore.Controllers
{
    public class ProductController : Controller
    {
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        IConfiguration _configuration;

        public ProductController(IProductService productService, IConfiguration configuration,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
        }
        [Route("products.html")]
        public IActionResult Index(int pageSize, string sortBy, int page = 1)
        {
            var model = new ShoppingViewModel();
            pageSize = _configuration.GetValue<int>("PageSize");

            model.PageSize = pageSize;
            model.SortType = sortBy;
            model.ProductCategory = _productCategoryService.GetAll();
            model.Product = _productService.GetAllPaging(string.Empty, page, pageSize);

            return View(model);

            //var shopViewModel = new ShopViewModel();
            //var products = _productService.GetAll();
            //var productCategoryModel = _productCategoryService.GetAll();
            //shopViewModel.ProductCategory = productCategoryModel;
            //shopViewModel.Products = products;
            //return View(shopViewModel);
        }

        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, string keyword, int? pageSize, string sortBy, int page = 1)
        {
            return View();
        }

        [Route("{alias}-p-{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 12);
            model.ProductImages = _productService.GetImages(id);
            model.Tags = _productService.GetProductTags(id);
            return View(model);
        }
    }
}