﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task UploadImageForCKEditor(IList<IFormFile> upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            DateTime now = DateTime.Now;
            if (upload.Count == 0)
            {
                await HttpContext.Response.WriteAsync("Yêu cầu nhập ảnh");
            }
            else
            {
                var file = upload[0];

                var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                await HttpContext.Response.WriteAsync("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", '" + Path.Combine(imageFolder, filename).Replace(@"\", @"/") + "');</script>");
            }
        }

        /// <summary>
        /// Upload image for form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadImage()
        {
            DateTime now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }
            else
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName = (TextHelper.ToUnsignFileName(file.Name))
                                    .Trim('"');

                var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return new OkObjectResult(Path.Combine(imageFolder, filename).Replace(@"\", @"/"));
            }
        }

      
        [HttpPost]
        [AllowAnonymous]
        public IActionResult UploadImageAPI()
        {
            DateTime now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }
            else
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return new OkObjectResult("{ \"path\":\"" + Path.Combine(imageFolder, filename).Replace(@"\", @"/") + "\"}");
            }
        }
    }
}