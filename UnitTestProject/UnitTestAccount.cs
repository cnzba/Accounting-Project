using CryptoService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UnitTestProject.Common;
using WebApp.Controllers;
using WebApp.Entities;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestAccount
    {
        private Mock<SignInManager<CBAUser>> _mockSignInManager;
        private Mock<UserManager<CBAUser>> _mockUserManager;
        private readonly Mock<ICryptography> _crypto;
        private Mock<IUserStore<CBAUser>> _mockUserStore;
        private readonly CBAContext _cbaContext;
        private readonly DbContextOptions<CBAContext> _dbOptions;
        private readonly AccountController _accountController;
        public UnitTestAccount()
        {
            _dbOptions = new DbContextOptionsBuilder<CBAContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _cbaContext = new CBAContext(_dbOptions);

            //Dependency Injection
            _crypto = new Mock<ICryptography>();
            _mockUserStore = new Mock<IUserStore<CBAUser>>();

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<CBAUser>>();

            _mockUserManager = new Mock<UserManager<CBAUser>>(_mockUserStore.Object,
                 null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<CBAUser>>(_mockUserManager.Object,
               contextAccessor.Object, userPrincipalFactory.Object, null, null, null);

            _accountController = new AccountController(_cbaContext, _crypto.Object, _mockSignInManager.Object, _mockUserManager.Object);
        }


        [TestMethod]
        public async Task Login()
        {
            CBAUser objCBAUser = ClsCommon.GetMockObject();

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<CBAUser>(), It.IsAny<string>(), false, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            Login objLogin = new Login()
            {
                Username = "guest@guest.com",
                Password = "guest1"
            };

            var result = await _accountController.Login(objLogin);
            Assert.IsNotNull(((ObjectResult)result).Value);
        }

        [TestMethod]
        public async Task Login_InValidUser()
        {
            //_mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<CBAUser>(), It.IsAny<string>(), true, true)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            Login objLogin = new Login()
            {
                Username = "guest11464hghghj@guest.com",
                Password = "guest14"
            };

            var result = await _accountController.Login(objLogin);
            Assert.AreEqual(((ObjectResult)result).StatusCode, 400);
        }

        [TestMethod]
        public async Task Logout()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProviderMock.Object
                }
            };

            var result = await _accountController.Logout();
            Assert.AreEqual(((Microsoft.AspNetCore.Mvc.StatusCodeResult)result).StatusCode, 200);
        }
    }
}
