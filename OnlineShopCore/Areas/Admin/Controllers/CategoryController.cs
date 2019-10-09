using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private ICategoryService _productCategoryService;

        public CategoryController(ICategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
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
        public IActionResult SaveEntity(CategoryViewModel productVm)
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
                }
                else
                {
                    _productCategoryService.Update(productVm);
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
                return new OkObjectResult(id);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }    
        #endregion Get Data API
    }
}