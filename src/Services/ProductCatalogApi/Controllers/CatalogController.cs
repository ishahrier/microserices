using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModels;

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
        [Route ("[action]")]
        public async Task<IActionResult> CatalogTypes () {
            var items = await CatalogDb.CatalogTypes.ToListAsync ();
            return Ok (items);
        }

        [HttpGet]
        [Route ("[action]")]
        public async Task<IActionResult> CatalogBrands () {

            var items = await CatalogDb.CatalogBrands.ToListAsync ();
            return Ok (items);
        }

        [HttpGet]
        [Route ("item/{id:int}")]

        public async Task<IActionResult> GetItemById (int id) {
            if (id <= 0) {
                return BadRequest ();
            }
            var item = await CatalogDb.CatalogItems.FirstOrDefaultAsync ();

            if (item != null) {
                item.PictureUrl = item.PictureUrl.Replace ("http://externalcatalogbaseurltobereplaced", Config.Value.ExternalCatalogBaseUrl);
                return Ok (item);
            }
            return NotFound ();
        }

        [HttpGet]
        [Route ("[action]")]
        public async Task<IActionResult> Items ([FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0) {
            var totalItem = await CatalogDb.CatalogItems.LongCountAsync ();
            var itemsOnPage = await CatalogDb.CatalogItems.OrderBy (x => x.Name)
                .Skip (pageIndex * pageSize)
                .Take (pageSize)
                .ToListAsync ();
            itemsOnPage = ChangeUrlPlaceHolder (itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem> (pageIndex, pageIndex, totalItem, itemsOnPage);
            return Ok (model);
        }

        [HttpGet]
        [Route ("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items (string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0) {
            var totalItem = await CatalogDb.CatalogItems
                .Where (x => x.Name.StartsWith (name))
                .LongCountAsync ();
            var itemsOnPage = await CatalogDb.CatalogItems
                .Where (x => x.Name.StartsWith (name))
                .OrderBy (x => x.Name)
                .Skip (pageIndex * pageSize)
                .Take (pageSize)
                .ToListAsync ();
            itemsOnPage = ChangeUrlPlaceHolder (itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem> (pageIndex, pageIndex, totalItem, itemsOnPage);
            return Ok (model);
        }

        [HttpGet]
        [Route ("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        public async Task<IActionResult> Items (int? catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0) {

            var root = CatalogDb.CatalogItems.AsQueryable ();
            if (catalogTypeId.HasValue) root = root.Where (x => x.CatalogTypeId == catalogTypeId);
            if (catalogBrandId.HasValue) root = root.Where (x => x.CatalogBrandId == catalogBrandId);

            var totalItem = await root.LongCountAsync ();
            var itemsOnPage = await root
                .OrderBy (x => x.Name)
                .Skip (pageIndex * pageSize)
                .Take (pageSize)
                .ToListAsync ();
            itemsOnPage = ChangeUrlPlaceHolder (itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem> (pageIndex, pageIndex, totalItem, itemsOnPage);
            return Ok (model);
        }

        [HttpPost]
        [Route ("items")]
        public async Task<IActionResult> CreateProduct ([FromBody] CatalogItem product) {
            var item = new CatalogItem () {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Price = product.Price,
                Description = product.Description,
                PictureFileName = product.PictureFileName,
                Name = product.Name
            };
            CatalogDb.CatalogItems.Add (item);
            await CatalogDb.SaveChangesAsync ();
            return CreatedAtAction (nameof (GetItemById), new { id = item.Id });
        }

        [HttpPut]
        [Route ("items")]
        public async Task<IActionResult> UpdateProduct ([FromBody] CatalogItem updateMe) {

            var item = await CatalogDb.CatalogItems.SingleOrDefaultAsync (x => x.Id == updateMe.Id);
            if (item == null) {
                return NotFound (new { Message = $"Item with id {updateMe.Id} doesn't exist." });
            }

            item = updateMe;
            CatalogDb.CatalogItems.Update (item);
            await CatalogDb.SaveChangesAsync ();
            return CreatedAtAction (nameof (GetItemById), new { id = item.Id });
        }

        [HttpDelete]
        [Route ("{id}")]
        public async Task<IActionResult> DelleteProduct ([FromBody] int id) {
            var item = await CatalogDb.CatalogItems.SingleOrDefaultAsync (x => x.Id == id);
            if (item == null) return NotFound (new { Message = $"Item with id {id} doesn't exist." });
            CatalogDb.CatalogItems.Remove (item);
            return NoContent ();

        }
        private List<CatalogItem> ChangeUrlPlaceHolder (List<CatalogItem> items) {
            items.ForEach (x => x.PictureUrl = x.PictureUrl.Replace ("http://externalcatalogbaseurltobereplaced", Config.Value.ExternalCatalogBaseUrl));
            return items;
        }

    }
}