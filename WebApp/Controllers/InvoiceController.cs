using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {

        private readonly CBAWEBACCOUNTContext cBAWEBACCOUNTContext;

        public InvoiceController(CBAWEBACCOUNTContext context)
        {
            cBAWEBACCOUNTContext = context;

            
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            return cBAWEBACCOUNTContext.Invoice.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var invoice = cBAWEBACCOUNTContext.Invoice.FirstOrDefault(t => t.Id == id);
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
