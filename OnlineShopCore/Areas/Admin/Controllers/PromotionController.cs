using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;
        private readonly IUnitOfWork _unitOfWork;

        public PromotionController(IPromotionService promotionService, IUnitOfWork unitOfWork)
        {
            _promotionService = promotionService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _promotionService.GetDetail(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _promotionService.GetAll();
            return new OkObjectResult(model);
        }

        [AllowAnonymous]
        [HttpPut]
        public IActionResult SetOutOfDate(int promotionID)
        {
            _promotionService.UpdateStatus(promotionID);

            return new OkResult();
        }

        [HttpPost]
        public IActionResult SaveEntity(PromotionViewModel promoVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (promoVm.DateExpired > DateTime.Now)
            {
                if (promoVm.Id == 0)
                {
                    _promotionService.Create(promoVm);
                }
                else
                {
                    _promotionService.Update(promoVm);
                }
                _promotionService.Save();
                return new OkObjectResult(promoVm);
            }
            else
                return new BadRequestObjectResult(promoVm);
        }
    }
}