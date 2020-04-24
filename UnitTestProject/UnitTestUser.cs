using AutoMapper;
using CryptoService;
using DotNetOpenAuth.OAuth.ChannelElements;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceUtil.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnitTestProject.AsyncClass;
using UnitTestProject.FakeIdentity;
using WebApp.Controllers;
using WebApp.Entities;
using WebApp.Models;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestUser
    {
        private readonly IEmailService _emailService;
        private readonly IOptions<EmailConfig> _emailConfig;
        private readonly DbContextOptions<CBAContext> _dbOptions;
        private readonly CBAContext _cbaContext;
        //Dependency Injection
        private readonly ICryptography _crypto;

        private Mock<IUserStore<CBAUser>> _mockUserStore;
        private Mock<UserManager<CBAUser>> _mockUserManager;
        private readonly Mock<ITokenGenerator> _tokenGenerator;
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
            _mockUserManager= new Mock<UserManager<CBAUser>>(_mockUserStore.Object,
                 null, null, null, null, null, null, null, null);
        }

        //[Fact]
        //public async Task GenerateConfirmationToken()
        //{
        //    // #### Setup ####
        //    // Reads: If GenerateToken method is called with the **exact** same instance as the user passed to the service
        //    _tokenGenerator.Setup(t => t.GenerateToken(It.Is(user)))
        //        // then return "abc123456" as token
        //        .Returns("abcd123456")
        //        // Verify that the method is called with the exact conditions from above, otherwise fail
        //        // i.e. if GenerateToken is called with a different instance of user, test will fail
        //        .Verifiable("ContainsKey not called.");

        //    // #### ACT ####

        //    // Pass the token generator mock to our account service
        //    var _accountService = new AccountService(_tokenGenerator.Object);
        //    var register = await _accountService.Register(_applicationUser, "password");
        //    var token = await _accountService.GenerateEmailConfirmationTokenAsync(userMock.Object.First());

        //    // #### VERIFY ####
        //    // Verify that GenerateToken method has been called with correct parameters
        //    _tokenGenerator.Verify();
        //    // verify that the GenerateEmailConfirmationTokenAsync returned the expected token abc123456
        //    Assert.Equals(token, "abcd123456");
        //}

        [TestMethod]
        public async Task PostUser()
        {
            //var userStoreMock = new Mock<IUserStore<CBAUser>>();

            //var _mockUserManager = new Mock<UserManager<CBAUser>>(userStoreMock.Object,
            //     null, null, null, null, null, null, null, null);

            var userController = new UserController(_cbaContext, _crypto, _mockUserManager.Object, _emailService, _emailConfig);

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

            //userStoreMock.Setup(s => s.CreateAsync(objCBAUser, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            //userStoreMock.Setup(s => s.GetUserNameAsync(objCBAUser, CancellationToken.None)).Returns(Task.FromResult(objCBAUser.UserName)).Verifiable();
            //userStoreMock.Setup(s => s.SetNormalizedUserNameAsync(objCBAUser, objCBAUser.UserName.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<CBAUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<CBAUser>()).Status);

            

            var result = await userController.PostUser(cbaUserRegDto);

            //var mockSet = CreateMoqDbSetUser(CreateMoqUsersData());

            //var mockContext = new Mock<CBAContext>();
            ////mockContext.Setup(c => c.CBAUser).Returns(mockSet.Object);

            //var mockStore = Mock.Of<IUserStore<CBAUser>>();
            //var mockUserManager = new Mock<UserManager<CBAUser>>(mockStore, null, null, null, null, null, null, null, null);

            //mockUserManager
            //    .Setup(x => x.CreateAsync(It.IsAny<CBAUser>(), It.IsAny<string>()))
            //    .ReturnsAsync(IdentityResult.Success);

            //var sut = new UserController(new CBAContext(), new Cryptography(), mockUserManager.Object, new ServiceUtil.Email.EmailService(), new EmailConfig());
            //var input = new NewUserInputBuilder().Build();

            ////Act
            //var actual = await sut.RegisterNewUser(input);

            ////Assert
            //actual
            //    .Should().NotBeNull()
            //    .And.Match<HttpResponseMessage>(_ => _.IsSuccessStatusCode == true);

        }

        [TestMethod]
        public async Task CheckUserexist() { 
        
        
        }


        [TestMethod]
        public async Task GetAllUserAsync()
        {

            //var mockSet = CreateMoqDbSetUser(CreateMoqUsersData());

            //var mockContext = new Mock<CBAContext>();
            //mockContext.Setup(c => c.CBAUser).Returns(mockSet.Object);

            //var service = new UserController(mockContext.Object, new Cryptography());

            //// Test Methodo API GetUsers() 
            //var actionResult = await service.GetUser();

            //var okObjectResult = actionResult as OkObjectResult;
            //Assert.IsNotNull(okObjectResult);

            //var lstUsers = okObjectResult.Value as List<CBAUser>;
            //Assert.IsNotNull(lstUsers);

            //// Test total itens
            //Assert.AreEqual(lstUsers.Count, 3);

            //// Test values
            //Assert.AreEqual(lstUsers[0].Email, CreateMoqUsersData().Where(a=> a.Id == lstUsers[0].Id).Select(a=> a.Email).FirstOrDefault());
            //Assert.AreEqual(lstUsers[0].Name, CreateMoqUsersData().Where(a => a.Id == lstUsers[0].Id).Select(a => a.Name).FirstOrDefault());
            //Assert.AreEqual(lstUsers[0].Password, CreateMoqUsersData().Where(a => a.Id == lstUsers[0].Id).Select(a => a.Password).FirstOrDefault());



        }


        private IQueryable<User> CreateMoqUsersData()
        {
            return new List<User>
            {
                new User { Id = 1, Active = true, Email = "Teste User001", Name = "test name1", Password = "123" },
                new User { Id = 2, Active = true, Email = "Teste User002", Name = "test name2", Password = "123" },
                new User { Id = 3, Active = true, Email = "Teste User003", Name = "test name3", Password = "123" },

            }.AsQueryable();
        }

        private Mock<DbSet<User>> CreateMoqDbSetUser(IQueryable<User> UsrData)
        {
            Mock<DbSet<User>> mockSet = new Mock<DbSet<User>>();

            mockSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<User>(UsrData.GetEnumerator()));

            mockSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<User>(UsrData.Provider));

            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(UsrData.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(UsrData.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => UsrData.GetEnumerator());

            return mockSet;

        }


        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

    }
}
