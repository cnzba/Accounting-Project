using WebApp.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Options;
using Microsoft.Extensions.Options;
using WebApp;
using CryptoService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace UnitTestProject
{
    [TestClass]
    public class InvoiceService_DeleteInvoiceShould
    {
        private readonly IOptions<CBAOptions> options;
        private readonly DbContextOptions<CBAContext> dboptions;
        private readonly Cryptography cryptography;

        public InvoiceService_DeleteInvoiceShould()
        {
            var ioptions = new Mock<IOptions<CBAOptions>>();
            var cbaoptions = new Mock<CBAOptions>().Object;

            ioptions.Setup(s => s.Value).Returns(cbaoptions);
            options = ioptions.Object;

            dboptions = new DbContextOptionsBuilder<CBAContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            cryptography = new Mock<Cryptography>().Object;
        }

        [TestMethod]
        public void DeleteInvoice_DeletesDraftInvoice()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var seeder = new CBASeeder(context, cryptography);
            seeder.Seed();

            var service = new InvoiceService(context, options);

            // act
            bool result = service.DeleteInvoice("20171005-001");

            // assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void DeleteInvoice_RejectsNonDraftInvoice()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var seeder = new CBASeeder(context, cryptography);
            seeder.Seed();

            var service = new InvoiceService(context, options);

            // act
            bool result = false;
            try { 
                service.DeleteInvoice("20171113-001");
            }
            catch (ArgumentException) { result = true; }

            // assert
            Assert.AreEqual(true, result);
        }
    }
}
