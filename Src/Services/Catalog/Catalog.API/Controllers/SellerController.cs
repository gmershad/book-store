using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public SellerController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateSellerAsync([FromBody] Seller seller)
        {
            _catalogContext.Sellers.Add(seller);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(SellerByIdAsync), new { id = seller.Id }, seller);
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(SellerByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Seller), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Seller>> SellerByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var seller = await _catalogContext.Sellers.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (seller != null)
            {
                return seller;
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> SellersAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var item = await GetSellersByIdsAsync(ids);
                if (!item.Any())
                {
                    return BadRequest("Ids values are invalid. It must be comma separated");
                }

                return Ok(item);
            }

            if(pageIndex <0 || pageSize < 0)
            {
                return BadRequest("Invalid page index or page size");
            }

            var totalItems = await _catalogContext.Sellers.LongCountAsync();
            var itemOnPage = await _catalogContext.Sellers
                .OrderBy(c => c.Name).Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();

            var data = new PaginatedItemsViewModel<Seller>(pageIndex, pageSize, totalItems, itemOnPage);
            return Ok(data);
        }

        [HttpGet]
        [Route("name/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Seller>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Seller>>> SellersWithNameAsync(string name, 
            [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 10)
        {
            var totalItems = await _catalogContext.Sellers.Where(c => c.Name.StartsWith(name)).LongCountAsync();
            if (totalItems == 0)
            {
                return NotFound(); 
            }

            var itemsOnPage = await _catalogContext.Sellers
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var data = new PaginatedItemsViewModel<Seller>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(data);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateSellerAsync([FromBody] Seller sellerToUpdate)
        {
            var sellerItem = await _catalogContext.Sellers.SingleOrDefaultAsync(i => i.Id == sellerToUpdate.Id);
            if (sellerItem == null)
            {
                return NotFound(new { Messsage = $"Items with id {sellerToUpdate.Id} not found." });
            }

            sellerItem = sellerToUpdate;
            _catalogContext.Sellers.Update(sellerItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(SellerByIdAsync), new { id = sellerToUpdate.Id }, null);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteSellerAsync(int id)
        {
            var product = _catalogContext.Sellers.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _catalogContext.Sellers.Remove(product);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }


        private async Task<List<Seller>> GetSellersByIdsAsync(string ids)
        {
            var nIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));
            if(!nIds.All(nIds => nIds.Ok))
            {
                return new List<Seller>();
            }

            var idsToSelect = nIds.Select(id => id.Value);
            var items = await _catalogContext.Sellers.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
            return items;
        }
    }
}
