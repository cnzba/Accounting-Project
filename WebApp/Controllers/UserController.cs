using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using CryptoService;
using System;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/User
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
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

        // GET: api/User/5
        [HttpGet("{login}")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] string login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Email.Equals(login));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest("Invalid ID");
            }

            if (LoginExists(user.Email))
            {
                user.Password = _crypto.HashMD5(user.Password);
                _context.Entry(user).State = EntityState.Modified;
            }

            try
            {
                _context.Entry(user).State = EntityState.Modified;


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (LoginExists(user.Email))
            {
                return BadRequest("Login Invalid");
            }


            user.Password = _crypto.HashMD5(user.Password);
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        internal User FindUser(string Name)
        {
            var user = new User();

            try
            {
                user = _context.User.Where(a => a.Email.ToLower().Equals(Name.ToLower())).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        internal async Task<User> GetUserByLogin(string login)
        {
            var user = new User();

            try
            {
                user = await _context.User.SingleOrDefaultAsync(a => a.Email.ToLower().Equals(login.ToLower().Trim()));
            }
            catch (Exception)
            {
                throw;
            }
            return user;

        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private bool LoginExists(string login)
        {
            return _context.User.Any(e => e.Email.ToLower().Equals(login.ToLower().Trim()));
        }

    }
}
