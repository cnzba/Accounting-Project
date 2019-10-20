using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Profiles
{
    public class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            CreateMap<Entities.Invoice, Models.InvoiceDto>();
            CreateMap<Entities.InvoiceLine, Models.InvoiceLineDto>();
            CreateMap<Models.InvoiceForCreationDto, Entities.Invoice>();
            CreateMap<Models.InvoiceLineDto, Entities.InvoiceLine>();
            CreateMap<Models.InvoiceForUpdateDto, Entities.Invoice>();
        }
    }
}
