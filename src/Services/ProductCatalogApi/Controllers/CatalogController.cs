using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;

namespace ProductCatalogApi.Controllers {
    [Produces ("application/json")]
    [Route ("api/Catalog")]
    public class CatalogController : Controller {
        private readonly CatalogContext CatalogDb;
        private readonly IOptionsSnapshot<CatalogSettings> Config;

        public CatalogController (CatalogContext catalogDb, IOptionsSnapshot<CatalogSettings> config) {
            CatalogDb = catalogDb;
            Config = config;
            catalogDb.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route ("{[action]}")]
        public async Task<IActionResult> CatalogTypes () {

            var items = await CatalogDb.CatalogTypes.ToListAsync ();
            return Ok (items);
        }

        [HttpGet]
        [Route ("{[action]}")]
        public async Task<IActionResult> CatalogBrands () {

            var items = await CatalogDb.CatalogBrands.ToListAsync ();
            return Ok (items);
        }
    }
}