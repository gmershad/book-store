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
    public class LanguageController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;

        public LanguageController(CatalogContext context)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ActionName(nameof(LanguageByIdAsync))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Language), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Language>> LanguageByIdAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var language = await _catalogContext.Languages.SingleOrDefaultAsync(ci => ci.Id == id);
            if (language != null)
            {
                return language;
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateLanguageAsync([FromBody] Language language)
        {
            if (string.IsNullOrEmpty(language.Type))
            {
                return BadRequest("Language type should not be empty.");
            }

            _catalogContext.Languages.Add(language);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(LanguageByIdAsync), new { id = language.Id }, language);
        }


        [Route("name/{languageType:minlength(1)}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Language), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Language>> LanguageByTypeAsync(string languageType)
        {
            if (string.IsNullOrEmpty(languageType))
            {
                return BadRequest();
            }

            var language = await _catalogContext.Languages.SingleOrDefaultAsync(ci => ci.Type.ToLower() == languageType.ToLower());
            if (language != null)
            {
                return language;
            }

            return NotFound();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateLanguageAsync([FromBody] Language languageToUpdate)
        {
            var languageItem = await _catalogContext.Languages.SingleOrDefaultAsync(i => i.Id == languageToUpdate.Id);
            if (languageItem == null)
            {
                return NotFound(new { Messsage = $"Items with id {languageToUpdate.Id} not found." });
            }

            languageItem = languageToUpdate;
            _catalogContext.Languages.Update(languageItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(LanguageByIdAsync), new { id = languageToUpdate.Id }, null);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteLanguageAsync(int id)
        {
            var item = _catalogContext.Languages.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _catalogContext.Languages.Remove(item);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }
    }

}
