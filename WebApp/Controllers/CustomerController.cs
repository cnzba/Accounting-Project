using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService, IMapper mapper, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            _logger = logger;
        }
    }
}