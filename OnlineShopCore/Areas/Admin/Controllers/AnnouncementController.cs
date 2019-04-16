using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Application.Interfaces;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        [HttpGet]
        public IActionResult GettAllPaging(int page, int pageSize)
        {
            var model = _announcementService.GetAllUnReadPaging(page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult MarkAsRead(string id)
        {
            var result = _announcementService.MarkAsRead(id);
            return new OkObjectResult(result);
        }
    }
}