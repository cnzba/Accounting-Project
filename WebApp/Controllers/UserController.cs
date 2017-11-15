using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using CryptoService;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly CBAContext _context;

        //Dependency Injection
        private readonly ICryptography _crypto;       

        public UserController(CBAContext context, ICryptography crypto)
        {
            _context = context;

            //Dependency Injection
            _crypto = crypto;
        }
        

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.User.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers([FromRoute] int id, [FromBody] User users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.Id)
            {
                return BadRequest("Invalid ID");
            }

            if(LoginExists(users.Login))
            {
                return BadRequest("Invalid Login");
            }



             users.Password = _crypto.HashMD5(users.Password);
            _context.Entry(users).State = EntityState.Modified;

            try
            {
                _context.Entry(users).State = EntityState.Modified;


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] User users)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (LoginExists(users.Login))
            {
                return BadRequest("Login Invalid");
            }


            users.Password = _crypto.HashMD5(users.Password);
            _context.User.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            _context.User.Remove(users);
            await _context.SaveChangesAsync();

            return Ok(users);
        }

        private bool UsersExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private bool LoginExists(string login)
        {
            return _context.User.Any(e => e.Login.ToLower().Equals(login.ToLower().Trim()));
        }

    }
}
