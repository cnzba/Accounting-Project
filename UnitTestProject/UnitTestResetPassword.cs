using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceUtil.Email;
using System.Threading.Tasks;
using WebApp.Controllers;
using WebApp.Entities;
using WebApp.Models;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestResetPassword
    {

        private readonly Mock<IEmailService> _emailService;
        private readonly IOptions<EmailConfig> _emailConfig;
        private Mock<IUserStore<CBAUser>> _mockUserStore;
        private Mock<UserManager<CBAUser>> _mockUserManager;
        private readonly ResetPasswordController _resetPasswordController;
        public UnitTestResetPassword()
        {
            var ioptions = new Mock<IOptions<EmailConfig>>();
            var cbaoptions = new Mock<EmailConfig>();

            ioptions.Setup(s => s.Value).Returns(cbaoptions.Object);
            _emailConfig = ioptions.Object;

            cbaoptions.SetupAllProperties();

            _mockUserStore = new Mock<IUserStore<CBAUser>>();
            _mockUserManager = new Mock<UserManager<CBAUser>>(_mockUserStore.Object,
                 null, null, null, null, null, null, null, null);

            _emailService = new Mock<IEmailService>();
            _emailService.Setup(x => x.SendEmail(It.IsAny<EmailConfig>(), It.IsAny<Email>())).ReturnsAsync(true);
            _resetPasswordController = new ResetPasswordController(_emailService.Object, _emailConfig, _mockUserManager.Object);
        }

        [TestMethod]
        public async Task PostVerifyToken_Valid()
        {
            VerifyTokenDto obj = new VerifyTokenDto()
            {
                Id = "cee2f64c-fe87-40ca-ac8b-",
                Token = "CfDJ8I9H5drWaGxHgWTL%2BERlWjzlReKkIFkFAtEfKwyOEM11WgvvjONokXL9sLJPyNf7lzlki8CfutXU7c8dUx8bxRmj4JYbeOFQaY0uacxy6eHN%2B0AOhX%2FPhdliED3P5qaIiVwWyr1W05a%2BOjgPkRXSQj7Ku0kSVgF4PcdJpL1oB3p2vjql1tYqqfIE7I5rJP5GBC%2BvAM5SOfVGOFry%2BBfA5F%2B3zzcL82H1PneaR2IXq64H"
            };

            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockUserManager.Setup(x => x.VerifyUserTokenAsync(It.IsAny<CBAUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var result = await _resetPasswordController.PostVerifyToken(obj);
            Assert.AreEqual(((StatusCodeResult)result).StatusCode, 200);
        }

        [TestMethod]
        public async Task PostVerifyToken_InvalidToken()
        {
            VerifyTokenDto obj = new VerifyTokenDto()
            {
                Id = "cee2f64c-fe87-40ca-ac8b-",
                Token = "CfDJ8I9H5drWaGxHgWTL%2BERlWjzlReKkIFkFAtEfKwyOEM11WgvvjONokXL9sLJPyNf7lzlki8CfutXU7c8dUx8bxRmj4JYbeOFQaY0uacxy6eHN%2B0AOhX%2FPhdliED3P5qaIiVwWyr1W05a%2BOjgPkRXSQj7Ku0kSVgF4PcdJpL1oB3p2vjql1tYqqfIE7I5rJP5GBC%2BvAM5SOfVGOFry%2BBfA5F%2B3zzcL82H1PneaR2IXq64H"
            };

            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockUserManager.Setup(x => x.VerifyUserTokenAsync(It.IsAny<CBAUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _resetPasswordController.PostVerifyToken(obj);
            Assert.AreEqual(((ObjectResult)result).Value, "Invalid token..!!");
        }

        [TestMethod]
        public async Task PostVerifyToken_InvalidUser()
        {
            VerifyTokenDto obj = new VerifyTokenDto()
            {
                Id = "cee2f64c-fe87-40ca-ac8b-",
                Token = "CfDJ8I9H5drWaGxHgWTL%2BERlWjzlReKkIFkFAtEfKwyOEM11WgvvjONokXL9sLJPyNf7lzlki8CfutXU7c8dUx8bxRmj4JYbeOFQaY0uacxy6eHN%2B0AOhX%2FPhdliED3P5qaIiVwWyr1W05a%2BOjgPkRXSQj7Ku0kSVgF4PcdJpL1oB3p2vjql1tYqqfIE7I5rJP5GBC%2BvAM5SOfVGOFry%2BBfA5F%2B3zzcL82H1PneaR2IXq64H"
            };

            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            //_mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync();
            _mockUserManager.Setup(x => x.VerifyUserTokenAsync(It.IsAny<CBAUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _resetPasswordController.PostVerifyToken(obj);
            Assert.AreEqual(((ObjectResult)result).Value, "Invalid User..!!");
        }

      
        [TestMethod]
        public async Task PostChangePassword()
        {
            ResetPasswordDto objPasswordModel = new ResetPasswordDto()
            {
                Id = "guest@guest.com",
                NewPassword = "12345678",
                ConfirmPassword = "12345678",
                Token = "CfDJ8I9H5drWaGxHgWTL%2BERlWjzlReKkIFkFAtEfKwyOEM11WgvvjONokXL9sLJPyNf7lzlki8CfutXU7c8dUx8bxRmj4JYbeOFQaY0uacxy6eHN%2B0AOhX%2FPhdliED3P5qaIiVwWyr1W05a%2BOjgPkRXSQj7Ku0kSVgF4PcdJpL1oB3p2vjql1tYqqfIE7I5rJP5GBC%2BvAM5SOfVGOFry%2BBfA5F%2B3zzcL82H1PneaR2IXq64H"
            };

            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<CBAUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var result = await _resetPasswordController.PostChangePassword(objPasswordModel);
            Assert.AreEqual(((Microsoft.AspNetCore.Mvc.StatusCodeResult)result).StatusCode, 200);
        }
    }
}
