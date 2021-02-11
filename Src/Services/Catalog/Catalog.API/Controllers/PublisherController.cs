using System;
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
    public class PublisherController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public PublisherController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateSellerAsync([FromBody] Publisher publisher)
        {
            _catalogContext.Publishers.Add(publisher);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(PublisherByIdAsync), new { id = publisher.Id }, publisher);
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(PublisherByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Publisher), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Publisher>> PublisherByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var publisher = await _catalogContext.Publishers.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (publisher != null)
            {
                return publisher;
            }

            return NotFound();
        }

        [Route("name/{name:minlength(1)}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Publisher), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Publisher>> PublisherByTypeAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var publisher = await _catalogContext.Publishers.SingleOrDefaultAsync(ci => ci.Name.ToLower() == name.ToLower());
            if (publisher != null)
            {
                return publisher;
            }

            return NotFound();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdatePublisherAsync([FromBody] Publisher publisherToUpdate)
        {
            var publisherItem = await _catalogContext.Publishers.SingleOrDefaultAsync(i => i.Id == publisherToUpdate.Id);
            if (publisherItem == null)
            {
                return NotFound(new { Messsage = $"Items with id {publisherItem.Id} not found." });
            }

            publisherItem = publisherToUpdate;
            _catalogContext.Publishers.Update(publisherItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(PublisherByIdAsync), new { id = publisherToUpdate.Id }, null);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeletePublisherAsync(int id)
        {
            var product = _catalogContext.Publishers.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _catalogContext.Publishers.Remove(product);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
