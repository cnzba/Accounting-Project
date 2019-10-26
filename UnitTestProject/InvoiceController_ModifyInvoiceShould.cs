using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApp;
using WebApp.Entities;
using WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WebApp.Services;
using WebApp.Models;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceController_ModifyInvoiceShould
    {
        private readonly ILogger<InvoiceController> logger;
        public InvoiceController_ModifyInvoiceShould()
        {
            logger = new Mock<ILogger<InvoiceController>>().Object;
        }

        [TestMethod]
        public void ModifyInvoice_Returns200Ok_WhenInvoiceModified()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var mapper = new Mock<IMapper>();
            var controller = new InvoiceController(service.Object, mapper.Object, logger);

            service.Setup(s => s.ModifyInvoice(It.IsAny<string>(), It.IsAny<InvoiceForUpdateDto>()));
            service.Setup(s => s.InvoiceExists(It.IsAny<string>())).Returns(true);

            //act
            var result = controller.ModifyInvoice("", new InvoiceForUpdateDto());

            //assert
            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public void ModifyInvoice_Returns404NotFound_WhenIncorrectRoute()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var mapper = new Mock<IMapper>();
            var controller = new InvoiceController(service.Object, mapper.Object, logger);

            //act
            var result = controller.ModifyInvoice("", new InvoiceForUpdateDto());

            //assert
            Assert.IsTrue(result is NotFoundResult);
        }
    }
}
