using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopCore.Controllers
{
    public class CertificatedController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public CertificatedController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Route(".well-known/acme-challenge/{id}")]
        public ActionResult LetsEncrypt(string id)
        {
            var file = Path.Combine(this._hostingEnvironment.WebRootPath, ".well-known", "acme-challenge", id);
            return PhysicalFile(file, "text/plain");
        }
    }
}