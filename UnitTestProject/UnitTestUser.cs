using CryptoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestProject.AsyncClass;
using WebApp.Controllers;
using WebApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public async Task GetAllUserAsync()
        {
             
            var mockSet = CreateMoqDbSetUser(CreateMoqUsersData());

            var mockContext = new Mock<CBAWEBACCOUNTContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            
            var service = new UsersController(mockContext.Object, new Cryptography());

            // Test Methodo API GetUsers() 
            var actionResult = await service.GetUsers();

            var okObjectResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var lstUsers = okObjectResult.Value as List<Users>;
            Assert.IsNotNull(lstUsers);
           
            // Test total itens
            Assert.AreEqual(lstUsers.Count, 3);
           
            // Test values
            Assert.AreEqual(lstUsers[0].Login, CreateMoqUsersData().Where(a=> a.IdUser == lstUsers[0].IdUser).Select(a=> a.Login).FirstOrDefault());
            Assert.AreEqual(lstUsers[0].Name, CreateMoqUsersData().Where(a => a.IdUser == lstUsers[0].IdUser).Select(a => a.Name).FirstOrDefault());
            Assert.AreEqual(lstUsers[0].Password, CreateMoqUsersData().Where(a => a.IdUser == lstUsers[0].IdUser).Select(a => a.Password).FirstOrDefault());



        }        


        private IQueryable<Users> CreateMoqUsersData()
        {
            return new List<Users>
            {
                new Users { IdUser = 1, Active = true, Login = "Teste User001", Name = "test name1", Password = "123" },
                new Users { IdUser = 2, Active = true, Login = "Teste User002", Name = "test name2", Password = "123" },
                new Users { IdUser = 3, Active = true, Login = "Teste User003", Name = "test name3", Password = "123" },

            }.AsQueryable();
        }

        private Mock<DbSet<Users>> CreateMoqDbSetUser(IQueryable<Users> UsrData)
        {
            Mock<DbSet<Users>> mockSet = new Mock<DbSet<Users>>();

            mockSet.As<IAsyncEnumerable<Users>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<Users>(UsrData.GetEnumerator()));

            mockSet.As<IQueryable<Users>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Users>(UsrData.Provider));

            mockSet.As<IQueryable<Users>>().Setup(m => m.Expression).Returns(UsrData.Expression);
            mockSet.As<IQueryable<Users>>().Setup(m => m.ElementType).Returns(UsrData.ElementType);
            mockSet.As<IQueryable<Users>>().Setup(m => m.GetEnumerator()).Returns(() => UsrData.GetEnumerator());

            return mockSet;

        }

    }
}
