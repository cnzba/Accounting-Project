using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiceUtil.Email;
using WebApp.Entities;
using System.IO;
using WebApp.Models;
using System;
using AutoMapper.Configuration;
using WebApp.Services;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResetPasswordController : Controller
    {
        private readonly ICryptography _crypto;
        private readonly CBAContext _context;
        private readonly IEmailService emailService;
        private readonly IEmailConfig emailConfig;
        private IHostingEnvironment _env;
        private ResetPasswordLinkExpireValidity configuration;
        private UserManager<CBAUser> _userManager;

        public ResetPasswordController(CBAContext context, ICryptography crypto, IEmailService emailService,
            IOptions<EmailConfig> emailConfig, IHostingEnvironment env, UserManager<CBAUser> userManager,
            IOptions<ResetPasswordLinkExpireValidity> serviceConfig)
        {
            _context = context;
            _crypto = crypto;
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
            _userManager = userManager;
           _env = env;
            configuration = serviceConfig.Value;
        }

        [HttpPost]
        public async Task<IActionResult> PostVerifyToken1([FromBody]VerifyTokenModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            //Get user details and verify token
            var user = await _context.User.SingleOrDefaultAsync(m => m.Id.Equals(obj.Id));

            if (user == null)
                return BadRequest("Invalid User");

            //if (!obj.Token.Equals(user.ForgetPasswordToken))
            //    return BadRequest("Invalid token");

            //TimeSpan totalDays = DateTime.Now - user.ForgetPasswordTokenGenerateDateTme;

            //int validityExpireDays = configuration.LinkExpireValidityInDays;
            //if (totalDays.TotalDays > validityExpireDays)
            //{
            //    return BadRequest("Token validation time has been expired..!!");
            //}
            return Ok();
        }


        [HttpPost]
        [Route("PostChangePassword")]
        public async Task<IActionResult> PostChangePassword([FromBody] ResetPasswordModel passwordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id.Equals(passwordModel.Id));

            if (user == null)
            {
                return NotFound("Invalid user");
            }

            //if (!passwordModel.Token.Equals(user.ForgetPasswordToken))
            //{
            //    return BadRequest("Invalid token");
            //}

            //user.Password = _crypto.HashMD5(passwordModel.NewPassword);
            //user.ForgetPasswordToken = string.Empty;
            //user.ForgetPasswordTokenGenerateDateTme = DateTime.Now;
            await _context.SaveChangesAsync();

            var pathToFile = _env.ContentRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "ChangePasswordTemplate.html";

            StreamReader SourceReader = System.IO.File.OpenText(pathToFile);
            string htmlBody = SourceReader.ReadToEnd();
            SourceReader.Close();

            Email emailContent = new Email()
            {
                To = user.Email,
                Subject = "Reset Password",
                Body = string.Format(htmlBody, user.Name)
            };
            await emailService.SendEmail(emailConfig, emailContent);
            return Ok("Password has been reset successfully");
            //return Ok();
        }
    }
}
