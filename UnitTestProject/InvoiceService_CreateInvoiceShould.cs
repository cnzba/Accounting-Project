using WebApp.Entities;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Options;
using Microsoft.Extensions.Options;
using WebApp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using WebApp.Services;
using AutoMapper;
using WebApp.Profiles;
using WebApp.Models;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceService_CreateInvoiceShould
    {
        private readonly IOptions<CBAOptions> options;
        private readonly DbContextOptions<CBAContext> dboptions;
        private readonly IMapper mapper;
        private readonly IPdfService pdf;

        public InvoiceService_CreateInvoiceShould()
        {
            var ioptions = new Mock<IOptions<CBAOptions>>();
            var cbaoptions = new Mock<CBAOptions>();

            var config = new MapperConfiguration(opts =>
                opts.AddProfile<InvoicesProfile>());

            pdf = new Mock<IPdfService>().Object;
            mapper = config.CreateMapper();

            cbaoptions.SetupAllProperties();
            cbaoptions.Object.CharitiesNumber = "1234567";
            cbaoptions.Object.GSTNumber = "11-111-111";

            ioptions.Setup(s => s.Value).Returns(cbaoptions.Object);
            options = ioptions.Object;

            dboptions = new DbContextOptionsBuilder<CBAContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [TestMethod]
        [Ignore("CBASeeder needs data for user organisations")]
        public void CreateInvoiceShould_CreateInvoice()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var service = new InvoiceService(context, options, mapper, pdf);

            var invoice = new InvoiceForCreationDto()
            {
                LoginId = "LoginFoo",
                ClientName = "Electrocal Commission",
                ClientContactPerson = "Glen Clarke",
                ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                Email = "ec@example.com",
                InvoiceLine = new List<InvoiceLineDto>()
                    {
                        new InvoiceLineDto()
                        {
                            Description = "Fundraising Dinner",
                            Amount = 25
                        },
                           new InvoiceLineDto()
                        {
                            Description = "Cake",
                            Amount = 35
                        }
                    }
            };

            // act
            var result = service.CreateInvoice(invoice);

            // assert
            Assert.IsTrue(result != null);
            var cleancontext = new CBAContext(dboptions);
            Assert.IsTrue(context.Invoice.Any());
            Assert.IsTrue(context.Invoice.FirstOrDefault().InvoiceLine.Count() == 2);
        }

        [TestMethod]
        public void CreateInvoiceShould_RequireProperties()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var service = new InvoiceService(context, options, mapper, pdf);
            var invoice = new InvoiceForCreationDto()
            {
                ClientName = "",
                ClientContactPerson = "Glen Clarke",
                ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                InvoiceLine = null
            };

            // act
            bool success = false;

            try
            {
                var result = service.CreateInvoice(invoice);
            }
            catch (ValidationException)
            {
                success = true;
            }
            // assert
            Assert.IsTrue(success);
        }

    }
}
