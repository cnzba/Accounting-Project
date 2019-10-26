using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp;
using Moq;
using WebApp.Controllers;
using WebApp.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WebApp.Profiles;
using WebApp.Services;
using WebApp.Models;

namespace UnitTestProject
{
    [TestClass]
    public class InvoiceController_CreateInvoiceShould
    {
        private readonly ILogger<InvoiceController> logger;
        private readonly IMapper mapper;
        private readonly InvoiceController controller;
        private readonly Mock<IInvoiceService> service;
        public InvoiceController_CreateInvoiceShould()
        {
            logger = new Mock<ILogger<InvoiceController>>().Object;
            var config = new MapperConfiguration(opts =>
                opts.AddProfile<InvoicesProfile>());

            mapper = config.CreateMapper();

            service = new Mock<IInvoiceService>();
            controller = new InvoiceController(service.Object, mapper, logger);
        }

        [TestMethod]
        public void CreateInvoice_Returns201AndInvoice_WhenInvoiceCreated()
        {
            //arrange
            service.Setup(s => s.CreateInvoice(It.IsAny<InvoiceForCreationDto>())).Returns(new Invoice());

            //act
            var result = controller.CreateInvoice(new InvoiceForCreationDto());

            //assert
            var createdAtAction = (result is CreatedAtActionResult);
            Assert.IsTrue(createdAtAction);

            var created = result as CreatedAtActionResult;
            Assert.IsTrue(created.Value is InvoiceDto);
            Assert.IsTrue(created.StatusCode == 201);
        }

        [TestMethod]
        public void CreateInvoice_Returns400BadRequest_WhenInvoiceNotCreated()
        {
            //arrange
            service.Setup(s => s.CreateInvoice(It.IsAny<InvoiceForCreationDto>())).Returns((Invoice)null);

            //act
            var result = controller.CreateInvoice(new InvoiceForCreationDto());

            //assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }
    }
}
