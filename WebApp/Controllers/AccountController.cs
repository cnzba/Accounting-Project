using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using CryptoService;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly CBAContext _context;

        //Dependency Injection
        private readonly ICryptography _crypto;

        public AccountController(CBAContext context, ICryptography crypto)
        {
            _context = context;

            //Dependency Injection
            _crypto = crypto;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login loginModel)
        {


            if (LoginUser(loginModel.Username, loginModel.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                //Just redirect to our index after logging in. 
                //return Redirect("/");
                return Ok();
            }
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    return Ok();
        //}

        private bool LoginUser(string username, string password)
        {
            UserController DbUser = new UserController(_context, _crypto);

            var User = DbUser.GetUser(username);

            if (User.Login.Equals(username) && User.Password.Equals(_crypto.HashMD5(password)))
            {
                return true;
            }
            else { return false; }
        }
    }
}
