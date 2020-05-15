using CryptoService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceUtil;
using ServiceUtil.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnitTestProject.Common;
using WebApp.Controllers;
using WebApp.Entities;
using WebApp.Models;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestUser
    {
        private readonly Mock<IEmailService> _emailService;
        private readonly IOptions<EmailConfig> _emailConfig;
        private readonly DbContextOptions<CBAContext> _dbOptions;
        private readonly CBAContext _cbaContext;
        private readonly Mock<ICryptography> _crypto;
        private Mock<IUserStore<CBAUser>> _mockUserStore;
        private Mock<UserManager<CBAUser>> _mockUserManager;
        private UserController _userController;
        private readonly Mock<ICreateReturnHTML> _createReturnHTML;
        public UnitTestUser()
        {
            var ioptions = new Mock<IOptions<EmailConfig>>();
            var cbaoptions = new Mock<EmailConfig>();

            ioptions.Setup(s => s.Value).Returns(cbaoptions.Object);
            _emailConfig = ioptions.Object;

            cbaoptions.SetupAllProperties();

            _dbOptions = new DbContextOptionsBuilder<CBAContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _cbaContext = new CBAContext(_dbOptions);

            _mockUserStore = new Mock<IUserStore<CBAUser>>();
            _mockUserManager = new Mock<UserManager<CBAUser>>(_mockUserStore.Object,
                 null, null, null, null, null, null, null, null);

            _emailService = new Mock<IEmailService>();
            _crypto = new Mock<ICryptography>();

            _createReturnHTML = new Mock<ICreateReturnHTML>();
            _userController = new UserController(_cbaContext, _crypto.Object, _mockUserManager.Object, _emailService.Object, _emailConfig, _createReturnHTML.Object);
        }


        [TestMethod]
        public async Task PostUser_Success()
        {
            //Mocking host address.
            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext();
            _userController.ControllerContext.HttpContext.Request.Host = new HostString("https://localhost:62856");

            var cbaUserRegDto = new UserRegDto()
            {
                Email = "guest@guest.com",
                Password = "12345678",
                FirstName = "Guest",
                LastName = "Test",
                PhoneNumber = "0225689994",
                OrgName = "Christchurch",
                OrgCode = "CHRCH",
                StreetAddrL1 = "1/32 catelina drive",
                StreetAddrL2 = "",
                City = "Auckland",
                Country = "Newzealand",
                OrgPhoneNumber = "0203388485",
                LogoURL = "",
                CharitiesNumber = "",
                GSTNumber = ""
            };

            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            //Mocking methods.
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<CBAUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<CBAUser>())).ReturnsAsync("CfDJ8I9H5drWaGxHgWTL+ERlWjwsoqt2f5ZNoc4xdQXmlFVvLV3crqNf8lyPN2+1i7zONZT+OR6gYFJZm6N3cjM2LnsnzobTpGLdznAhTQ3LEE/sW/F9b7AtIT2cvGDVFbsjSJN0GUTdXaJFiZR8yrBI2fggnAB5rDqGBnq3UOyJN0qa68Xwj6bsifkPFy25xRlOnNh83MPVFwzUuGwOpsWMsaUnaTb+XCWGQgBUzSISlKyv+wLS7mU1+iOKqpWpI/HPSg==");

            var result = await _userController.PostUser(cbaUserRegDto);
            Assert.AreEqual(((ObjectResult)result).Value, "succeed");
        }

        [TestMethod]
        public async Task PostUser_Failed()
        {
            //Mocking host address.
            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext();
            _userController.ControllerContext.HttpContext.Request.Host = new HostString("https://localhost:62856");

            var cbaUserRegDto = new UserRegDto()
            {
                Email = "guest@guest.com",
                Password = "12345678",
                FirstName = "Guest",
                LastName = "Test",
                PhoneNumber = "0225689994",
                OrgName = "Christchurch",
                OrgCode = "CHRCH",
                StreetAddrL1 = "1/32 catelina drive",
                StreetAddrL2 = "",
                City = "Auckland",
                Country = "Newzealand",
                OrgPhoneNumber = "0203388485",
                LogoURL = "",
                CharitiesNumber = "",
                GSTNumber = ""
            };

            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            IdentityError err = new IdentityError();
            err.Description = "Failed to create the user";

            //Mocking methods.
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<CBAUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(err));
            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<CBAUser>())).ReturnsAsync("CfDJ8I9H5drWaGxHgWTL+ERlWjwsoqt2f5ZNoc4xdQXmlFVvLV3crqNf8lyPN2+1i7zONZT+OR6gYFJZm6N3cjM2LnsnzobTpGLdznAhTQ3LEE/sW/F9b7AtIT2cvGDVFbsjSJN0GUTdXaJFiZR8yrBI2fggnAB5rDqGBnq3UOyJN0qa68Xwj6bsifkPFy25xRlOnNh83MPVFwzUuGwOpsWMsaUnaTb+XCWGQgBUzSISlKyv+wLS7mU1+iOKqpWpI/HPSg==");

            var result = await _userController.PostUser(cbaUserRegDto);
            Assert.AreEqual(((ObjectResult)result).Value, "Failed to create the user");
        }


        [TestMethod]
        public async Task CheckUserExist_UserIsAvailable()
        {
            CBAUser objCBAUser = ClsCommon.GetMockObject();

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);

            var result = await _userController.CheckUserExist("guest@guest.com");
            Assert.IsNotNull(((ObjectResult)result).Value, "Exist");

        }


        [TestMethod]
        public async Task CheckUserExist_UserIsNotAvaialable()
        {
            CBAUser objCBAUser = ClsCommon.GetMockObject();

            //IdentityError err = new IdentityError();
            //err.Description = "Failed to create the user";
            //_mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(err));

            var result = await _userController.CheckUserExist("guest12@guest.com");
            Assert.AreEqual(((ObjectResult)result).Value, "NotExist");

        }


        [TestMethod]
        public async Task ConfirmMail_ValidToken()
        {
            CBAUser objCBAUser = ClsCommon.GetMockObject();

            string mockToken = "CfDJ8I9H5drWaGxHgWTL + ERlWjwsoqt2f5ZNoc4xdQXmlFVvLV3crqNf8lyPN2 + 1i7zONZT + OR6gYFJZm6N3cjM2LnsnzobTpGLdznAhTQ3LEE / sW / F9b7AtIT2cvGDVFbsjSJN0GUTdXaJFiZR8yrBI2fggnAB5rDqGBnq3UOyJN0qa68Xwj6bsifkPFy25xRlOnNh83MPVFwzUuGwOpsWMsaUnaTb + XCWGQgBUzSISlKyv + wLS7mU1 + iOKqpWpI / HPSg == ";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<CBAUser>(), mockToken)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<CBAUser>())).ReturnsAsync(IdentityResult.Success);

            //Mocking host address.
            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext();
            _userController.ControllerContext.HttpContext.Request.Host = new HostString("https://localhost:62856");

            var result = await _userController.ConfirmEmail(objCBAUser.Id, mockToken);
            Assert.AreEqual(((ObjectResult)result).Value, "succeed");

        }


        [TestMethod]
        public async Task ConfirmMail_InvalidToken()
        {
            CBAUser objCBAUser = ClsCommon.GetMockObject();

            string mockToken = "CfDJ8I9H5drWaGxHgWTL + ERlWjwsoqt2f5ZNoc4xdQXmlFVvLV3crqNf8lyPN2 + 1i7zONZT + OR6gYFJZm6N3cjM2LnsnzobTpGLdznAhTQ3LEE / sW / F9b7AtIT2cvGDVFbsjSJN0GUTdXaJFiZR8yrBI2fggnAB5rDqGBnq3UOyJN0qa68Xwj6bsifkPFy25xRlOnNh83MPVFwzUuGwOpsWMsaUnaTb + XCWGQgBUzSISlKyv + wLS7mU1 + iOKqpWpI / HPSg == ";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            _mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<CBAUser>(), mockToken + "1234")).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<CBAUser>())).ReturnsAsync(IdentityResult.Success);

            var result = await _userController.ConfirmEmail(objCBAUser.Id, mockToken);
            Assert.AreEqual(((ObjectResult)result).StatusCode, 500);

        }



        [TestMethod]
        public void UploadLogo()
        {
            Mock<HttpRequest> mockRequest = new Mock<HttpRequest>();
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(Directory.GetCurrentDirectory());
            string path = Path.Combine(appRoot.ToString(), "Icons", "CBA-Logo.jpeg");

            FileInfo fileInfo = new FileInfo(path);
            byte[] buffer = new byte[fileInfo.Length];

            var httpContext = new DefaultHttpContext();

            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, fileInfo.Length, "Data", path)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/text",
                ContentDisposition = "attachment; filename=CBA-Logoupdated-01-1.jpeg"
            };

            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _userController.ControllerContext = new ControllerContext(actx);
            _userController.Request.Headers.Add("Content-Disposition", "attachment; filename=CBA-Logoupdated-01-1.jpeg");

            var result = _userController.UploadLogo();
        }

        [TestMethod]
        public async Task GetUser_UserIsLoggedIn()
        {
            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                     {
                        new Claim(ClaimTypes.Name, "guest@guest.com"),
                        new Claim("UserID", "16ce682d-2bd5-45df-bc08-1f21e12f68b0"),
                     }, "mock"));

            _userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(objCBAUser);
            var result = await _userController.GetUser();

            Assert.IsNotNull(((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
        }



        //[TestMethod]
        //public async Task GetAllUserAsync()
        //{

        //    //var mockSet = CreateMoqDbSetUser(CreateMoqUsersData());

        //    //var mockContext = new Mock<CBAContext>();
        //    //mockContext.Setup(c => c.CBAUser).Returns(mockSet.Object);

        //    //var service = new UserController(mockContext.Object, new Cryptography());

        //    //// Test Methodo API GetUsers() 
        //    //var actionResult = await service.GetUser();

        //    //var okObjectResult = actionResult as OkObjectResult;
        //    //Assert.IsNotNull(okObjectResult);

        //    //var lstUsers = okObjectResult.Value as List<CBAUser>;
        //    //Assert.IsNotNull(lstUsers);

        //    //// Test total itens
        //    //Assert.AreEqual(lstUsers.Count, 3);

        //    //// Test values
        //    //Assert.AreEqual(lstUsers[0].Email, CreateMoqUsersData().Where(a=> a.Id == lstUsers[0].Id).Select(a=> a.Email).FirstOrDefault());
        //    //Assert.AreEqual(lstUsers[0].Name, CreateMoqUsersData().Where(a => a.Id == lstUsers[0].Id).Select(a => a.Name).FirstOrDefault());
        //    //Assert.AreEqual(lstUsers[0].Password, CreateMoqUsersData().Where(a => a.Id == lstUsers[0].Id).Select(a => a.Password).FirstOrDefault());
        //}


        //private Mock<DbSet<User>> CreateMoqDbSetUser(IQueryable<User> UsrData)
        //{
        //    Mock<DbSet<User>> mockSet = new Mock<DbSet<User>>();

        //    mockSet.As<IAsyncEnumerable<User>>()
        //        .Setup(m => m.GetEnumerator())
        //        .Returns(new TestAsyncEnumerator<User>(UsrData.GetEnumerator()));

        //    mockSet.As<IQueryable<User>>()
        //        .Setup(m => m.Provider)
        //        .Returns(new TestAsyncQueryProvider<User>(UsrData.Provider));

        //    mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(UsrData.Expression);
        //    mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(UsrData.ElementType);
        //    mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => UsrData.GetEnumerator());

        //    return mockSet;

        //}

    }
}
