using CryptoService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceUtil.Email;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnitTestProject.Common;
using WebApp.Controllers;
using WebApp.Entities;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestForgetPassword
    {
        private readonly Mock<IEmailService> _emailService;
        private readonly IOptions<EmailConfig> _emailConfig;
        private readonly DbContextOptions<CBAContext> _dbOptions;
        private Mock<IUserStore<CBAUser>> _mockUserStore;
        private Mock<UserManager<CBAUser>> _mockUserManager;
        private ForgotPasswordController _forgetPasswordController;
        public UnitTestForgetPassword()
        {
            var ioptions = new Mock<IOptions<EmailConfig>>();
            var cbaoptions = new Mock<EmailConfig>();

            ioptions.Setup(s => s.Value).Returns(cbaoptions.Object);
            _emailConfig = ioptions.Object;

            cbaoptions.SetupAllProperties();

            _dbOptions = new DbContextOptionsBuilder<CBAContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
           
            _mockUserStore = new Mock<IUserStore<CBAUser>>();
            _mockUserManager = new Mock<UserManager<CBAUser>>(_mockUserStore.Object,
                 null, null, null, null, null, null, null, null);

            _emailService = new Mock<IEmailService>();
           
            _forgetPasswordController = new ForgotPasswordController(_emailService.Object, _emailConfig, _mockUserManager.Object);
        }

        [TestMethod]
        public async Task PostForgotPassword()
        {
            EmailModel obj = new EmailModel()
            {
                Email = "ripal92.parikh@gmail.com"
            };
            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            string token = "CfDJ8I9H5drWaGxHgWTL%2BERlWjzlReKkIFkFAtEfKwyOEM11WgvvjONokXL9sLJPyNf7lzlki8CfutXU7c8dUx8bxRmj4JYbeOFQaY0uacxy6eHN%2B0AOhX%2FPhdliED3P5qaIiVwWyr1W05a%2BOjgPkRXSQj7Ku0kSVgF4PcdJpL1oB3p2vjql1tYqqfIE7I5rJP5GBC%2BvAM5SOfVGOFry%2BBfA5F%2B3zzcL82H1PneaR2IXq64H";
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<CBAUser>())).ReturnsAsync(token);

            //Mocking host address.
            _forgetPasswordController.ControllerContext = new ControllerContext();
            _forgetPasswordController.ControllerContext.HttpContext = new DefaultHttpContext();
            _forgetPasswordController.ControllerContext.HttpContext.Request.Host = new HostString("localhost:62856");
            _forgetPasswordController.ControllerContext.HttpContext.Request.Scheme = "https";

            var result = await _forgetPasswordController.PostForgotPassword(obj);
            Assert.AreEqual(((ObjectResult)result).Value, "An email has been sent with instruction to reset your password...!!");

        }


        [TestMethod]
        public async Task PostForgotPassword_UserDoesNotExist()
        {
            EmailModel obj = new EmailModel()
            {
                Email = "ripal92.parikh@gmail.com"
            };

            string token = "CfDJ8I9H5drWaGxHgWTL%2BERlWjzlReKkIFkFAtEfKwyOEM11WgvvjONokXL9sLJPyNf7lzlki8CfutXU7c8dUx8bxRmj4JYbeOFQaY0uacxy6eHN%2B0AOhX%2FPhdliED3P5qaIiVwWyr1W05a%2BOjgPkRXSQj7Ku0kSVgF4PcdJpL1oB3p2vjql1tYqqfIE7I5rJP5GBC%2BvAM5SOfVGOFry%2BBfA5F%2B3zzcL82H1PneaR2IXq64H";
            _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<CBAUser>())).ReturnsAsync(token);

            //Mocking host address.
            _forgetPasswordController.ControllerContext = new ControllerContext();
            _forgetPasswordController.ControllerContext.HttpContext = new DefaultHttpContext();
            _forgetPasswordController.ControllerContext.HttpContext.Request.Host = new HostString("localhost:62856");
            _forgetPasswordController.ControllerContext.HttpContext.Request.Scheme = "https";

            var result = await _forgetPasswordController.PostForgotPassword(obj);
            Assert.AreEqual(((ObjectResult)result).Value, "User does not exist..!!");
        }
    }
}
