using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private readonly CBAContext cBAWEBACCOUNTContext;
        private readonly CBAOptions options;

        // context; options via DI
        public InvoiceController(CBAContext context, IOptions<CBAOptions> optionsAccessor)
        {
            cBAWEBACCOUNTContext = context;
            options = optionsAccessor.Value;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            return cBAWEBACCOUNTContext.Invoice.Include("InvoiceLine").ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var invoice = cBAWEBACCOUNTContext.Invoice.Include("InvoiceLine").FirstOrDefault(t => t.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }
            return new ObjectResult(invoice);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
