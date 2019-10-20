using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp;
using Moq;
using WebApp.Controllers;
using WebApp.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using WebApp.Services;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceController_DeleteInvoiceShould
    {
        private readonly ILogger<InvoiceController> logger;
        public InvoiceController_DeleteInvoiceShould()
        {
            logger = new Mock<ILogger<InvoiceController>>().Object;
        }

        [TestMethod]
        public void DeleteInvoice_Returns404NotFound_WhenInvalidInvoiceNumber()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var mapper = new Mock<IMapper>();
            var controller = new InvoiceController(service.Object, mapper.Object, logger);

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
            var mapper = new Mock<IMapper>();
            var controller = new InvoiceController(service.Object, mapper.Object, logger);

            service.Setup(s => s.InvoiceExists(It.IsAny<String>())).Returns(true);
            service.Setup(s => s.DeleteInvoice(It.IsAny<String>())).Throws(new ArgumentException());

            //act
            var result = controller.DeleteInvoice("xxx");

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

    }
}
