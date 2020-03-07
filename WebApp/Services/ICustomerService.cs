using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.Services
{
    public interface ICustomerService
    {
        Customer Create(CustomerDto dto);
    }
}
