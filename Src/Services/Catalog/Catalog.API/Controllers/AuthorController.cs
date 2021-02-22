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
    public class AuthorController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public AuthorController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateAuthorAsync([FromBody] Author author)
        {
            if (string.IsNullOrEmpty(author.Name))
            {
                return BadRequest("Author name should not be empty");
            }

            _catalogContext.Authors.Add(author);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(AuthorByIdAsync), new { id = author.Id }, author);
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(AuthorByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Author>> AuthorByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var author = await _catalogContext.Authors.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (author != null)
            {
                return author;
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AuthorsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var item = await GetAuthorsByIdsAsync(ids);
                if (!item.Any())
                {
                    return BadRequest("Ids values are invalid. It must be comma separated");
                }

                return Ok(item);
            }

            if (pageIndex <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page index or page size");
            }

            var totalItems = await _catalogContext.Authors.LongCountAsync();
            var itemOnPage = await _catalogContext.Authors
                .OrderBy(c => c.Name).Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();

            var data = new PaginatedItemsViewModel<Author>(pageIndex, pageSize, totalItems, itemOnPage);
            return Ok(data);
        }


        [HttpGet]
        [Route("name/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Author>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Author>>> AuthorsWithNameAsync(string name,
            [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.Authors.Where(c => c.Name.StartsWith(name)).LongCountAsync();
            if (totalItems == 0)
            {
                return NotFound();
            }

            var itemsOnPage = await _catalogContext.Authors
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var data = new PaginatedItemsViewModel<Author>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(data);
        }

        private async Task<List<Author>> GetAuthorsByIdsAsync(string ids)
        {
            var noIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), value: x));
            if (!noIds.All(n => n.Ok))
            {
                return new List<Author>();
            }

            var idsToSelect = noIds.Select(id => id.value);
            var items = await _catalogContext.Authors.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
            return items;
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateAuthorAsync([FromBody] Author authorToUpdate)
        {
            var authorItem = await _catalogContext.Authors.SingleOrDefaultAsync(i => i.Id == authorToUpdate.Id);
            if (authorItem == null)
            {
                return NotFound(new { Messsage = $"Items with id {authorToUpdate.Id} not found." });
            }

            authorItem = authorToUpdate;
            _catalogContext.Authors.Update(authorItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(AuthorByIdAsync), new { id = authorToUpdate.Id }, null);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteAuthorAsync(int id)
        {
            var item = _catalogContext.Authors.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _catalogContext.Authors.Remove(item);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
