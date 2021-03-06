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
using Microsoft.Extensions.Logging;
using ServiceUtil.Email;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using System.Net;
using ServiceUtil;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private UserManager<CBAUser> _userManager;
        //private SignInManager<CBAUser> _signInManager;
        private readonly CBAContext _context;
        private readonly IEmailService _emailService;
        private readonly IEmailConfig _emailConfig;
        private readonly ICreateReturnHTML _createReturnHTML;

        //Dependency Injection
        private readonly ICryptography _crypto;
        //private ILogger _logger;

        public UserController(CBAContext context,
            ICryptography crypto,
            UserManager<CBAUser> userManager,
            //SignInManager<CBAUser> signInManager
            //ILogger logger,
            IEmailService emailService,
            IOptions<EmailConfig> emailConfig,
            ICreateReturnHTML createReturnHTML
            )
        {
            _context = context;
            //Dependency Injection
            _crypto = crypto;
            _userManager = userManager;
            //_logger = logger;
            _emailService = emailService;
            _emailConfig = emailConfig.Value;
            _createReturnHTML = createReturnHTML;
        }


        // GET: api/User/User
        /// <summary>
        /// Get a user by UserID which is in the token.
        /// </summary>
        /// <returns>If found user, return the user, else return NotFound</returns>
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

        /// <summary>
        /// Check if the user exists by email.
        /// </summary>
        /// <param name="email">Email to be checked</param>
        /// <returns>If user exist, return "Exist", or return "NotExist"</returns>
        [HttpGet,Route("CheckUserExist/{email}")]
        public async Task<IActionResult> CheckUserExist([FromRoute] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok("NotExist");
            return Ok("Exist");
        }


        /// <summary>
        /// 1. Save the user and organisation in database
        /// 2. If success, send a confirmation email to the user.
        /// </summary>
        /// <param name="regUser"></param>
        /// <returns></returns>
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
                if (result != null && result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(cbaUser);                    
                    var hostAddress = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    var queryPrarms = new Dictionary<string, string>() {
                        {"userId", cbaUser.Id + ""},
                        {"token", code + ""}
                    };

                    string callbackUrl = QueryHelpers.AddQueryString($"{hostAddress}/api/user/confirmEmail", queryPrarms);
                    var pathToFile = Directory.GetCurrentDirectory()
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "EmailTemplates"
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "ConfirmRegEmailTemplate.html";

                    string htmlBody = "";

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        htmlBody = await SourceReader.ReadToEndAsync();
                    };

                    Email emailContent = new Email()
                    {
                        To = cbaUser.Email,
                        Subject = $"CBA user validation email for {cbaUser.FirstName} {cbaUser.LastName}",
                        Body = string.Format(htmlBody, cbaUser.FirstName, callbackUrl)
                    };                    
                    var sentEmailRes = await _emailService.SendEmail(_emailConfig, emailContent);
                    
                    return sentEmailRes? Ok("succeed") : StatusCode(500, "Failed to send confirmation email, please contact CBA");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        //_logger.LogError(error.ToString());                        
                        Console.WriteLine(error.ToString());
                    }
                    return StatusCode(500, "Failed to create the user");
                }

            }
            catch (Exception ex)
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
        /// <summary>
        /// Call this method when new regestered user click confirm in the confirmation email.        ///  
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns>A succeed HTML message if succeed, or 500 if failed</returns>
        [HttpGet, Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery]string token)
        {
            var confirmUser = await _userManager.FindByIdAsync(userId);
            if (confirmUser != null)
            {
                var result = await _userManager.ConfirmEmailAsync(confirmUser, token);
                if (result != null && result.Succeeded)
                {
                    confirmUser.EmailConfirmed = true;
                    confirmUser.IsActive = true;
                    await _userManager.UpdateAsync(confirmUser);
                    string message = $"Email confirmation succeed, click <a href='https://{Request.Host.Value}'> here</a> to login";
                    var responseBody = _createReturnHTML.GetHTML(message);
                    Response.ContentType = "text/html";
                    await Response.Body.WriteAsync(responseBody, 0, responseBody.Length);
                }
                return StatusCode(500, "Failed to verify the user.");
            }
            else
            {
                return StatusCode(500, "The user does not exist.");
            }
        }

        /// <summary>
        /// 1. Upload the logo file to server
        /// </summary>
        /// <returns>If a valid file, then return full path of the logo file.</returns>
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
                        ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
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
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error:{ex}");
            }
        }

        #region Previous codes, could be useful in the future. So keep this code for now.

        // GET: api/Users
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
        #endregion


    }
}
