using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents;
using Catalog.API.Migrations.Events;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

        public BookCatalogController(CatalogContext context, ICatalogIntegrationEventService catalogIntegrationEventService)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(BookByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Book>> BookByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var book = await _catalogContext.Books.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (book != null)
            {
                return book;
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateBookAsync([FromBody] Book book)
        {
            if (string.IsNullOrEmpty(book.Name))
            {
                return BadRequest("Book name should not be empty");
            }

            _catalogContext.Books.Add(book);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(BookByIdAsync), new { id = book.Id }, book);
        }


        [HttpGet]
        public async Task<IActionResult> BooksAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var item = await GetBooksByIdsAsync(ids);
                if (!item.Any())
                {
                    return BadRequest("Ids values are invalid. It must be comma separated");
                }

                return Ok(item);
            }

            if (pageIndex < 0 || pageSize < 0)
            {
                return BadRequest("Invalid page index or page size");
            }

            var totalItems = await _catalogContext.Authors.LongCountAsync();
            var itemOnPage = await _catalogContext.Books
                .OrderBy(c => c.Name).Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();

            var data = new PaginatedItemsViewModel<Book>(pageIndex, pageSize, totalItems, itemOnPage);
            return Ok(data);
        }

        private async Task<List<Book>> GetBooksByIdsAsync(string ids)
        {
            var noIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), value: x));
            if (!noIds.All(n => n.Ok))
            {
                return new List<Book>();
            }

            var idsToSelect = noIds.Select(id => id.value);
            var items = await _catalogContext.Books.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
            return items;
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteBookAsync(int id)
        {
            var book = _catalogContext.Books.SingleOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            _catalogContext.Books.Remove(book);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }

        [Route("format")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateBookFormatAsync([FromBody] BookFormat bookFormat)
        {
            if (string.IsNullOrEmpty(bookFormat.Type))
            {
                return BadRequest("Book format type should not be empty");
            }

            _catalogContext.BookFormats.Add(bookFormat);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(BookFormatByIdAsync), new { id = bookFormat.Id }, bookFormat);
        }

        [Route("format/id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(BookFormatByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BookFormat>> BookFormatByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var bookFormat = await _catalogContext.BookFormats.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (bookFormat != null)
            {
                return bookFormat;
            }

            return NotFound();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateOfferAsync([FromBody] Book bookToUpdate)
        {
            var bookItem = await _catalogContext.Books.SingleOrDefaultAsync(i => i.Id == bookToUpdate.Id);
            if (bookItem == null)
            {
                return NotFound(new { Messsage = $"Book with id {bookToUpdate.Id} not found." });
            }

            var oldPrice = bookItem.Price;
            var raiseOfPriceChangedEvent = oldPrice != bookToUpdate.Price;

            bookItem = bookToUpdate;
            _catalogContext.Books.Update(bookItem);

            if (raiseOfPriceChangedEvent)
            {
                var offerPercentChangeEvent = new BookPriceChangedIntegrationEvent(bookToUpdate.Id, bookToUpdate.Price, oldPrice);
                await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(offerPercentChangeEvent);
                await _catalogIntegrationEventService.PublishThroughEventBusAsync(offerPercentChangeEvent);
            }
            else
            {
                await _catalogContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(BookByIdAsync), new { id = bookToUpdate.Id }, null);
        }

    }
}
