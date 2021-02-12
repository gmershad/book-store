using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public GenresController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(GeneresByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Genres), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Genres>> GeneresByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var bookGeneres  = await _catalogContext.BookGenres.
                SingleOrDefaultAsync(ci => ci.Id == id);

            if (bookGeneres != null)
            {
                return bookGeneres;
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateGeneresAsync([FromBody] Genres genres)
        {
            _catalogContext.BookGenres.Add(genres);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GeneresByIdAsync), new { id = genres.Id }, genres);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateGenresAsync([FromBody] Genres genresToUpdate)
        {
            var genresItem = await _catalogContext.BookGenres.SingleOrDefaultAsync(i => i.Id == genresToUpdate.Id);
            if (genresItem == null)
            {
                return NotFound(new { Messsage = $"Generes with id {genresToUpdate.Id} not found." });
            }

            genresItem = genresToUpdate;
            _catalogContext.BookGenres.Update(genresItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GeneresByIdAsync), new { id = genresToUpdate.Id }, null);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteGeneresAsync(int id)
        {
            var item = _catalogContext.BookGenres.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _catalogContext.BookGenres.Remove(item);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
