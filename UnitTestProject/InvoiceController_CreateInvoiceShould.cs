using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp;
using Moq;
using WebApp.Controllers;
using WebApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceController_CreateInvoiceShould
    {
        private readonly ILoggerFactory logger;
        public InvoiceController_CreateInvoiceShould()
        {
            logger = new Mock<ILoggerFactory>().Object;
        }

        [TestMethod]
        public void CreateInvoice_Returns400BadRequest_WhenModelStateIsInvalid()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            controller.ModelState.AddModelError("fake", "required");
            service.Setup(s => s.CreateInvoice(It.IsAny<DraftInvoice>())).Returns(new Invoice());

            //act
            var result = controller.CreateInvoice(new DraftInvoice());

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public void CreateInvoice_Returns201AndInvoice_WhenInvoiceCreated()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            service.Setup(s => s.CreateInvoice(It.IsAny<DraftInvoice>())).Returns(new Invoice());

            //act
            var result = controller.CreateInvoice(new DraftInvoice());

            //assert
            var createdAtAction = (result is CreatedAtActionResult);
            Assert.IsTrue(createdAtAction);

            var created = result as CreatedAtActionResult;
            Assert.IsTrue(created.Value is Invoice);
            Assert.IsTrue(created.StatusCode == 201);
        }

        [TestMethod]
        public void CreateInvoice_Returns400BadRequest_WhenInvoiceNotCreated()
        {
            //arrange
            var service = new Mock<IInvoiceService>();
            var controller = new InvoiceController(service.Object, logger);

            service.Setup(s => s.CreateInvoice(It.IsAny<DraftInvoice>())).Returns((Invoice)null);

            //act
            var result = controller.CreateInvoice(new DraftInvoice());

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }
    }
}
