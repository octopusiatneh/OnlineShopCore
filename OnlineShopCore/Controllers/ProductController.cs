using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShopCore.Controllers
{
    public class ProductController : Controller
    {
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        IBillService _billService;
        IConfiguration _configuration;

        public ProductController(IProductService productService, IConfiguration configuration,
           IBillService billService,
           IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _billService = billService;
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
        }

        [Route("search.html")]
        public IActionResult Search(string keyword, int pageSize, string sortBy, int page = 1)
        {
            var model = new SearchResultViewModel();
            pageSize = _configuration.GetValue<int>("PageSize");

            model.PageSize = pageSize;
            model.SortType = sortBy;
            model.ProductCategory = _productCategoryService.GetAll();
            model.Product = _productService.GetAllPaging(keyword, page, pageSize);

            return View(model);
        }
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetProductForAutocomplete()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var model = _productService.GetAll().Where(p => p.Name.ToLower().Contains(term)).Select(p => p.Name).ToList();
                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
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
            model.Colors = _billService.GetColors().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.Sizes = _billService.GetSizes().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }
    }
}