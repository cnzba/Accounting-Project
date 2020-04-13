using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using CryptoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Account/")]
    public class AccountController : Controller
    {
        private readonly CBAContext _context;
        private SignInManager<CBAUser> _signInManager;
        private UserManager<CBAUser> _userManager;

        //Dependency Injection
        private readonly ICryptography _crypto;

        public AccountController(CBAContext context, 
            ICryptography crypto,
            SignInManager<CBAUser> signInManager,
            UserManager<CBAUser> userManager)
        {
            _context = context;

            //Dependency Injection
            _crypto = crypto;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]Login loginModel)
        {
            //loginModel.RememberMe = false;
            //var (LoginOk, userName) = LoginUser(loginModel.Username, loginModel.Password);
            //var result = await _signInManager.PasswordSignInAsync(loginModel.Username, 
            //                                   loginModel.Password,
            //                                   loginModel.RememberMe,
            //                                   lockoutOnFailure:false);
            var curUser = await _userManager.FindByNameAsync(loginModel.Username);
            if (curUser == null) return BadRequest(new { message = "The user is not exist" });

            bool isValidPassword = await _userManager.CheckPasswordAsync(curUser, loginModel.Password);
            if (!isValidPassword) return BadRequest(new { message = "The password is not correct" });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("UserID", curUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return Ok(new { token });
            //if (isValidUser)
            //{
            //    var claims = new List<Claim> {
            //        new Claim(ClaimTypes.Name, loginModel.Username)
            //    };

            //    var userIdentity = new ClaimsIdentity(claims, "login");

            //    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            //    await HttpContext.SignInAsync(principal);

            //    //return Ok(value: userName);
            //    return Ok(value:curUser.Email);
            //}
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        private (bool, string) LoginUser(string username, string password)
        {
            //UserController DbUser = new UserController(_context, _crypto);

            //var User = DbUser.FindUser(username);

            //if (User == null)
            //{
            //    return (false, string.Empty);
            //}

            //if (User.Email.Equals(username) && User.Password.Equals(_crypto.HashMD5(password)))
            //    return (true, User.Email);
            //else
            //    return (false, "");
            return (true, "OK");
        }
    }
}
