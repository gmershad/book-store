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
    [Route("api/[controller]")]
    [ApiController]
    public class BookCatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public BookCatalogController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
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
    }
}
