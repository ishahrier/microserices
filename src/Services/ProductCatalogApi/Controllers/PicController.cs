using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogApi.Controllers {

    [Produces ("application/json")]
    [Route ("api/Pic")]
    public class PicController : Controller {

        protected IHostingEnvironment Env { get; }
        public PicController (IHostingEnvironment env) {
            Env = env;
        }

        [HttpGet]
        [Route ("{id}")]
        public IActionResult GetImage (int id) {
            var webRoot = Env.WebRootPath;
            var path = Path.Combine (webRoot + "/Pics/", $"shoes-{id}.png");
            var buffer = System.IO.File.ReadAllBytes (path);
            return File (buffer, "images/png");
        }


    }
}