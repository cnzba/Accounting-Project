using System.Linq;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using CryptoService;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class CBASeeder
    {
        CBAContext _context;
        IHostingEnvironment _hosting;
        ICryptography _cryptography;

        public CBASeeder(CBAContext context, IHostingEnvironment hosting, ICryptography cryptography)
        {
            _context = context;
            _hosting = hosting;
            _cryptography = cryptography;
        }
        public void Seed()
        {
            if (!_context.Invoice.Any())
            {
                var invoice1 = new Invoice()
                {
                    InvoiceNumber = "INV-00050",
                    DateCreated = new DateTime(2017, 10, 5),
                    IssueeOrganization = "Electrocal Commission",
                    IssueeCareOf = "Glen Clarke",
                    ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                    Status = InvoiceStatus.New,
                    Gst = 0,
                    GstNumber = "96-712-561",
                    CharitiesNumber = "CC20097",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Fundraising Dinner",
                            Amount = 25
                        }
                    }
                };

                var invoice2 = new Invoice()
                {
                    InvoiceNumber = "INV-00051",
                    DateCreated = new DateTime(2017, 11, 13),
                    DateDue = new DateTime(2017, 11, 27),
                    IssueeOrganization = "Jason Carpets",
                    ClientContact = "297 Moorhouse Ave\\r\\nSydenham\\r\\nChristchurch 8011",
                    Status = InvoiceStatus.Sent,
                    Gst = 0,
                    GstNumber = "96-712-561",
                    CharitiesNumber = "CC20097",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Fundraising Dinner",
                            Amount = 25
                        }
                    }
                };

                var invoice3 = new Invoice()
                {
                    InvoiceNumber = "INV-00052",
                    DateCreated = new DateTime(2017, 9, 9),
                    DateDue = new DateTime(2017, 9, 30),
                    IssueeOrganization = "Transtellar",
                    ClientContact = "52 Solmine Ave\\r\\nRiccarton\\r\\nChristchurch 8025",
                    Status = InvoiceStatus.Paid,
                    Gst = 9.78M,
                    GstNumber = "96-712-561",
                    CharitiesNumber = "CC20097",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Fundraising Dinner",
                            Amount = 21.74M
                        },
                        new InvoiceLine()
                        {
                            Description = "Bookkeeping 2 hours @21.74 per hour",
                            Amount = 43.48M
                        }
                    }
                };
      
                _context.Add(invoice1);
                _context.Add(invoice2);
                _context.Add(invoice3);

                _context.SaveChanges();
            }

            if (!_context.User.Any())
            {
                var user1 = new User()
                {
                    Login = "guest",
                    Name = "guest",
                    Password = _cryptography.HashMD5("guest"),
                    Active = true
                };

                var user2 = new User()
                {
                    Login = "helersonlage@gmail.com",
                    Name = "Helerson Lage",
                    Password = "68eacb97d86f0c4621fa2b0e17cabd8c",
                    Active = true
                };

                _context.User.Add(user1);
                _context.User.Add(user2);

                _context.SaveChanges();
            }
        }
    }
}

