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
    public class InvoiceService_ModifyInvoiceShould
    {
        private readonly IOptions<CBAOptions> options;
        private readonly DbContextOptions<CBAContext> dboptions;
        private readonly Cryptography cryptography;

        public InvoiceService_ModifyInvoiceShould()
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
        public void ModifyInvoice_ReplacesInvoiceLines()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var seeder = new CBASeeder(context, cryptography);
            seeder.Seed();

            var service = new InvoiceService(context, options);
            var originalInvoice = context.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.InvoiceNumber == "20171005-001");

            var modifiedInvoice = new DraftInvoice()
            {
                InvoiceNumber = "20171005-001",
                InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "New Dinner",
                            Amount = 50
                        },
                           new InvoiceLine()
                        {
                            Description = "New Cookie",
                            Amount = 10
                        }
                    }
            };

            var expectedLineCount = context.InvoiceLine.Count()
                - originalInvoice.InvoiceLine.Count()
                + modifiedInvoice.InvoiceLine.Count(); 

            // act
            var result = service.ModifyInvoice(modifiedInvoice);
            var lineCount = context.InvoiceLine.Count();

            // assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedLineCount, lineCount);
        }

    }
}
