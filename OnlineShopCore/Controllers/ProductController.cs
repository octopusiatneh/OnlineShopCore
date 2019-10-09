﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Models.ProductViewModels;
using System.Linq;

namespace OnlineShopCore.Controllers
{
    public class ProductController : Controller
    {
        private ICategoryService _productCategoryService;
        private IProductService _productService;
        private IBillService _billService;
        private IConfiguration _configuration;

        public ProductController(IProductService productService, IConfiguration configuration,
           IBillService billService,
           ICategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _billService = billService;
        }

        [Route("products")]
        public IActionResult Index(int pageSize, string sortBy, int page = 1)
        {
            var model = new ShoppingViewModel();
            pageSize = _configuration.GetValue<int>("PageSize");

            model.PageSize = pageSize;
            model.SortBy = sortBy;
            model.Category = _productCategoryService.GetAll();
            model.Product = _productService.GetAllPaging(string.Empty, page, pageSize);

            return View(model);
        }

        [Route("filter")]
        public IActionResult Filter(int pageSize, string filter, int page = 1)
        {
            var model = new SearchResultViewModel();
            pageSize = _configuration.GetValue<int>("PageSize");

            model.PageSize = pageSize;
            model.SortBy = filter;
            model.Category = _productCategoryService.GetAll();
            model.Product = _productService.Filter(filter, page, pageSize);

            return View(model);
        }

        [Route("search")]
        public IActionResult Search(string keyword, int pageSize, string sortBy, int page = 1)
        {
            var model = new SearchResultViewModel();
            pageSize = _configuration.GetValue<int>("PageSize");

            model.PageSize = pageSize;
            model.SortBy = sortBy;
            model.Category = _productCategoryService.GetAll();
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

        [Route("{alias}-st-{id}", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 12);
            model.ProductImages = _productService.GetImages(id);
            //model.Tags = _productService.GetProductTags(id);
            //model.Colors = _billService.GetColors().Select(x => new SelectListItem()
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //}).ToList();        
            return View(model);
        }
    }
}