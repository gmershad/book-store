using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        // GET: api/Author
        [HttpGet]
        public IEnumerable<string> Get1()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Author/5
        [HttpGet("{id}", Name = "Get")]
        public string Get1(int id)
        {
            return "value";
        }

        // POST: api/Author
        [HttpPost]
        public void Post1([FromBody] string value)
        {
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public void Put1(int id, [FromBody] string value)
        { 
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete1(int id)
        {
        }
    }
}
