using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private ISlideService _slideService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SlideController(ISlideService slideService, IHostingEnvironment hostingEnvironment)
        {
            _slideService = slideService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _slideService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _slideService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(SlideViewModel slideVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (slideVm.Id == 0)
                {
                    _slideService.Add(slideVm);
                }
                else
                {
                    _slideService.Update(slideVm);
                }
                _slideService.Save();
                return new OkObjectResult(slideVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _slideService.Delete(id);
                _slideService.Save();

                return new OkObjectResult(id);
            }
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var images = _slideService.GetImages(productId);
            return new OkObjectResult(images);
        }

        #endregion AJAX API
    }
}