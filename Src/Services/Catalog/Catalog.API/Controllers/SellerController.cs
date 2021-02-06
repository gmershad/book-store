using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.Model;
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

        [Route("{id:int}")]
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
    }
}
