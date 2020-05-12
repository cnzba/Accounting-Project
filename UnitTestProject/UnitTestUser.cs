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
using WebApp.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public async Task GetAllUserAsync()
        {
             
            //var mockSet = CreateMoqDbSetUser(CreateMoqUsersData());

            //var mockContext = new Mock<CBAContext>();
            //mockContext.Setup(c => c.User).Returns(mockSet.Object);
            
            //var service = new UserController(mockContext.Object, new Cryptography());

            //// Test Methodo API GetUsers() 
            //var actionResult = await service.GetUser();

            //var okObjectResult = actionResult as OkObjectResult;
            //Assert.IsNotNull(okObjectResult);

            //var lstUsers = okObjectResult.Value as List<User>;
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

    }
}
