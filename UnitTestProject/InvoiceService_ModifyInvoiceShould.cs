using WebApp.Entities;
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
using WebApp.Services;
using AutoMapper;
using WebApp.Profiles;
using WebApp.Models;
using Microsoft.Extensions.Logging;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceService_ModifyInvoiceShould
    {
        private readonly IOptions<CBAOptions> options;
        private readonly DbContextOptions<CBAContext> dboptions;
        private readonly Cryptography cryptography;
        private readonly IMapper mapper;
        private readonly IPdfService pdf;
        private readonly ILogger<InvoiceService> logger = new Mock<ILogger<InvoiceService>>().Object;

        public InvoiceService_ModifyInvoiceShould()
        {
            var config = new MapperConfiguration(opts =>
                opts.AddProfile<InvoicesProfile>());

            pdf = new Mock<IPdfService>().Object;

            mapper = config.CreateMapper();
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

            var service = new InvoiceService(context, options, mapper, pdf, logger);
            var originalInvoice = context.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.InvoiceNumber == "ABNZ000420");

            var modifiedInvoice = new InvoiceForUpdateDto()
            {
                // all fields should be the same except InvoiceLine
                ClientName = originalInvoice.ClientName,
                ClientContact = originalInvoice.ClientContact,
                ClientContactPerson = originalInvoice.ClientContactPerson,
                DateDue = originalInvoice.DateDue,
                Email = originalInvoice.Email,
                PurchaseOrderNumber = originalInvoice.PurchaseOrderNumber,

                InvoiceLine = new List<InvoiceLineDto>()
                    {
                        new InvoiceLineDto()
                        {
                            Description = "New Dinner",
                            Amount = 50
                        },
                           new InvoiceLineDto()
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
            service.ModifyInvoice("ABNZ000420", modifiedInvoice);
            var lineCount = context.InvoiceLine.Count();

            // assert
            Assert.AreEqual(expectedLineCount, lineCount);
        }

        [TestMethod]
        public void ModifyInvoice_UpdatesInvoiceLines_WhenOnlyInvoiceLinesModified()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var seeder = new CBASeeder(context, cryptography);
            seeder.Seed();

            var service = new InvoiceService(context, options, mapper, pdf, logger);
            Invoice invoiceToUpdate = context.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.InvoiceNumber == "ABNZ000420");

            InvoiceForUpdateDto invoice = new InvoiceForUpdateDto()
            {
                // all fields should be the same except InvoiceLine
                ClientName = invoiceToUpdate.ClientName,
                ClientContact = invoiceToUpdate.ClientContact,
                ClientContactPerson = invoiceToUpdate.ClientContactPerson,
                DateDue = invoiceToUpdate.DateDue,
                Email = invoiceToUpdate.Email,
                PurchaseOrderNumber = invoiceToUpdate.PurchaseOrderNumber
            };

            invoice.InvoiceLine = new List<InvoiceLineDto>()
                    {
                        new InvoiceLineDto()
                        {
                            Description = "New Dinner",
                            Amount = 50
                        },
                           new InvoiceLineDto()
                        {
                            Description = "New Cookie",
                            Amount = 10
                        }
                    };

            // act
            service.ModifyInvoice("ABNZ000420", invoice);

            // assert
            var cleancontext = new CBAContext(dboptions);
            Assert.AreEqual(2, cleancontext.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.InvoiceNumber == "ABNZ000420").InvoiceLine.Count());
        }
    }
}
