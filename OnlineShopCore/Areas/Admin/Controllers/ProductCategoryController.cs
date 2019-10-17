using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.EF;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private IProductCategoryService _productCategoryService;
        private readonly AppDbContext _context;

        public ProductCategoryController(IProductCategoryService productCategoryService, AppDbContext context)
        {
            _productCategoryService = productCategoryService;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["sortOrder"] = _productCategoryService.GetAll();
            return View();
        }

        #region Get Data API

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productCategoryService.GetById(id);

            return new ObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    _productCategoryService.Add(productVm);
                    //logging activity
                    var userName = User.Identity.Name;
                    _context.Loggings.Add(new Logging(DateTime.Now, userName, "create new category"));
                    _context.SaveChanges();
                }
                else
                {
                    _productCategoryService.Update(productVm);
                    //logging activity
                    var userName = User.Identity.Name;
                    _context.Loggings.Add(new Logging(DateTime.Now, userName, "update category"));
                    _context.SaveChanges();
                }
                _productCategoryService.Save();
                return new OkObjectResult(productVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _productCategoryService.Delete(id);
                _productCategoryService.Save();
                //logging activity
                var userName = User.Identity.Name;
                _context.Loggings.Add(new Logging(DateTime.Now, userName, "create new product"));
                _context.SaveChanges();
                return new OkObjectResult(id);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.UpdateParentId(sourceId, targetId, items);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.ReOrder(sourceId, targetId);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        #endregion Get Data API
    }
}