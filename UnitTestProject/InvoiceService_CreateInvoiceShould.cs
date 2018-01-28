using WebApp.Models;
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

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceService_CreateInvoiceShould
    {
        private readonly IOptions<CBAOptions> options;
        private readonly DbContextOptions<CBAContext> dboptions;

        public InvoiceService_CreateInvoiceShould()
        {
            var ioptions = new Mock<IOptions<CBAOptions>>();
            var cbaoptions = new Mock<CBAOptions>();

            cbaoptions.SetupAllProperties();
            cbaoptions.Object.CharitiesNumber = "1234567";
            cbaoptions.Object.GSTNumber = "11-111-111";

            ioptions.Setup(s => s.Value).Returns(cbaoptions.Object);
            options = ioptions.Object;

            dboptions = new DbContextOptionsBuilder<CBAContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [TestMethod]
        public void CreateInvoiceShould_CreateInvoice()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var service = new InvoiceService(context, options);
            var invoice = new DraftInvoice()
            {
                ClientName = "Electrocal Commission",
                ClientContactPerson = "Glen Clarke",
                ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Fundraising Dinner",
                            Amount = 25
                        },
                           new InvoiceLine()
                        {
                            Description = "Cake",
                            Amount = 35
                        }
                    }
            };

            // act
            var result = service.CreateInvoice(invoice);

            // assert
            Assert.IsTrue(result);
            var cleancontext = new CBAContext(dboptions);
            Assert.IsTrue(context.Invoice.Any());
            Assert.IsTrue(context.Invoice.FirstOrDefault().InvoiceLine.Count() == 2);
        }

        [TestMethod]
        public void CreateInvoiceShould_RequireProperties()
        {
            // arrange
            var context = new CBAContext(dboptions);
            var service = new InvoiceService(context, options);
            var invoice = new DraftInvoice()
            {
                ClientName = "",
                ClientContactPerson = "Glen Clarke",
                ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                InvoiceLine = null
            };

            // act
            bool success = false; 

            try { 
                var result = service.CreateInvoice(invoice);
            }
            catch(ValidationException ve)
            {
                success = true;
            }
            // assert
            Assert.IsTrue(success);
        }

    }
}
