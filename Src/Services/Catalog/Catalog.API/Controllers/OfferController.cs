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
    public class OfferController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public OfferController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateOfferAsync([FromBody] Offer offer)
        {
            _catalogContext.Offers.Add(offer);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(OfferByIdAsync), new { id = offer.Id }, offer);
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(OfferByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Publisher), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Offer>> OfferByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var offer = await _catalogContext.Offers.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (offer != null)
            {
                return offer;
            }

            return NotFound();
        }

        [Route("name/{name:minlength(1)}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Publisher), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Offer>> OfferByTypeAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var offer = await _catalogContext.Offers.SingleOrDefaultAsync(ci => ci.Name.ToLower() == name.ToLower());
            if (offer != null)
            {
                return offer;
            }

            return NotFound();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateOfferAsync([FromBody] Offer offerToUpdate)
        {
            var offerItem = await _catalogContext.Offers.SingleOrDefaultAsync(i => i.Id == offerToUpdate.Id);
            if (offerItem == null)
            {
                return NotFound(new { Messsage = $"Items with id {offerToUpdate.Id} not found." });
            }

            offerItem = offerToUpdate;
            _catalogContext.Offers.Update(offerItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(OfferByIdAsync), new { id = offerToUpdate.Id }, null);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteOfferAsync(int id)
        {
            var offer = _catalogContext.Offers.SingleOrDefault(x => x.Id == id);

            if (offer == null)
            {
                return NotFound();
            }

            _catalogContext.Offers.Remove(offer);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
