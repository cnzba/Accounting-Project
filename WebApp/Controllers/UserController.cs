using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using CryptoService;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using System.IO;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private UserManager<CBAUser> _userManager;
        //private SignInManager<CBAUser> _signInManager;
        private readonly CBAContext _context;

        //Dependency Injection
        private readonly ICryptography _crypto;

        public UserController(CBAContext context, 
            ICryptography crypto,
            UserManager<CBAUser> userManager
            //SignInManager<CBAUser> signInManager
            )
        {
            _context = context;

            //Dependency Injection
            _crypto = crypto;
            _userManager = userManager;


        }

        // GET: api/User
        [HttpGet]
        [Authorize]
        [Route("Users")]
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

        // GET: api/User/User
        [HttpGet()]
        [Authorize]
        [Route("User")]
        public async Task<IActionResult> GetUser()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var user = await _context.User.SingleOrDefaultAsync(m => m.Email.Equals(login));
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }

        [HttpGet,Route("CheckUserExist/{email}")]
        public async Task<IActionResult> CheckUserExist([FromRoute] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok("NotExist");
            return Ok("Exist");
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
        public async Task<IActionResult> PostUser([FromBody]UserRegDto regUser)
        {
            var cbaUser = new CBAUser()
            {
                Email = regUser.Email,
                FirstName = regUser.FirstName,
                LastName = regUser.LastName,
                PhoneNumber = regUser.PhoneNumber,
                UserName = regUser.Email,
                Organisation = new Organisation
                {
                    Name = regUser.OrgName,
                    Code = regUser.OrgCode,
                    StreetAddressOne = regUser.StreetAddrL1,
                    StreetAddressTwo = regUser.StreetAddrL2,
                    City = regUser.City,
                    Country = regUser.Country,
                    PhoneNumber = regUser.OrgPhoneNumber,
                    Logo = regUser.LogoURL,
                    CharitiesNumber = regUser.CharitiesNumber,
                    GSTNumber = regUser.GSTNumber,
                    CreatedAt = DateTime.Now,                     
                }
            };

            try
            {
                var result = await _userManager.CreateAsync(cbaUser, regUser.Password);
                return Ok(result);

            }catch(Exception ex)
            {
                throw ex;
            }

            #region Old code
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (LoginExists(user.Email))
            //{
            //    return BadRequest("Login Invalid");
            //}


            //user.Password = _crypto.HashMD5(user.Password);
            //_context.User.Add(user);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUsers", new { id = user.Id }, user); 
            #endregion

        }

        [HttpPost, Route("uploadLogo")]
        [DisableRequestSizeLimit]

        public IActionResult UploadLogo()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine(@"Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = 
                        DateTime.Now.ToFileTime().ToString() +
                        ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');                        ;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }                
            }catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error:{ex}");
            }
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
