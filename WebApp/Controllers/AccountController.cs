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
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Account/")]
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

            var (LoginOk, userName) = LoginUser(loginModel.Username, loginModel.Password);

            if (LoginOk)
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


                return Ok(value: userName);
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }


        [Authorize]
        [HttpGet()]
        [Route(template: "api/Account/GetUserLogged")]
        public async Task<IActionResult> GetUserLogged()
        {
            UserController DbUser = new UserController(_context, _crypto);
            ClaimsPrincipal currentUser = this.User;

            var user = await DbUser.GetUserByLogin(currentUser.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }


            var SafeUserToReturn = new User() { Login = user.Login, Name = user.Name, Active = user.Active };
            return Ok(SafeUserToReturn);
        }


        private (bool, string) LoginUser(string username, string password)
        {
            UserController DbUser = new UserController(_context, _crypto);

            var User = DbUser.GetUser(username);

            if (User.Login.Equals(username) && User.Password.Equals(_crypto.HashMD5(password)))
                return (true, User.Name);
            else
                return (false, string.Empty);
        }
    }
}
