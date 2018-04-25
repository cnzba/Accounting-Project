using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp;
using Moq;
using WebApp.Controllers;
using WebApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceController_DeleteInvoiceShould
    {
        private readonly ILoggerFactory logger;
        public InvoiceController_DeleteInvoiceShould()
        {
            logger = new Mock<ILoggerFactory>().Object;
        }

        [TestMethod]
        public void DeleteInvoice_Returns404NotFound_WhenInvalidInvoiceNumber()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            service.Setup(s => s.InvoiceExists(It.IsAny<String>())).Returns(false);

            //act
            var result = controller.DeleteInvoice("xxx");

            //assert
            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void DeleteInvoice_Returns400BadRequest_WhenInvoiceNotDraft()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            service.Setup(s => s.InvoiceExists(It.IsAny<String>())).Returns(true);
            service.Setup(s => s.DeleteInvoice(It.IsAny<String>())).Throws(new ArgumentException());

            //act
            var result = controller.DeleteInvoice("xxx");

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

    }
}
