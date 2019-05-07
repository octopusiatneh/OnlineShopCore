using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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