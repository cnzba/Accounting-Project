using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApp;
using WebApp.Models;
using WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceController_ModifyInvoiceShould
    {
        private readonly ILoggerFactory logger;
        public InvoiceController_ModifyInvoiceShould()
        {
            logger = new Mock<ILoggerFactory>().Object;
        }

        [TestMethod]
        public void ModifyInvoice_Returns400BadRequest_WhenModelStateIsInvalid()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            controller.ModelState.AddModelError("fake", "required");
            service.Setup(s => s.ModifyInvoice(It.IsAny<DraftInvoice>())).Returns(true);

            //act
            var result = controller.ModifyInvoice("", new DraftInvoice() { InvoiceNumber = "" });

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public void ModifyInvoice_Returns200Ok_WhenInvoiceModified()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            service.Setup(s => s.ModifyInvoice(It.IsAny<DraftInvoice>())).Returns(true);

            //act
            var result = controller.ModifyInvoice("", new DraftInvoice() { InvoiceNumber = "" });

            //assert
            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public void ModifyInvoice_Returns400BadRequest_WhenInvoiceNotModified()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            service.Setup(s => s.ModifyInvoice(It.IsAny<DraftInvoice>())).Returns(false);

            //act
            var result = controller.ModifyInvoice("", new DraftInvoice() { InvoiceNumber = "" });

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public void ModifyInvoice_Returns400BadRequest_WhenIncorrectRoute()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            //act
            var result = controller.ModifyInvoice("", new DraftInvoice() { InvoiceNumber = "x" });

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }
    }
}
