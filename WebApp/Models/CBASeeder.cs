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
        ICryptography _cryptography;

        public CBASeeder(CBAContext context, ICryptography cryptography)
        {
            _context = context;
            _cryptography = cryptography;
        }
        public void Seed()
        {
            if (!_context.Invoice.Any())
            {
                var invoice1 = new Invoice()
                {
                    InvoiceNumber = "20171005-001",
                    DateCreated = new DateTime(2017, 10, 5),
                    ClientName = "Electrocal Commission",
                    ClientContactPerson = "Glen Clarke",
                    ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                    Status = InvoiceStatus.Draft,
                    GstRate = .15m,
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
                    InvoiceNumber = "20171113-001",
                    DateCreated = new DateTime(2017, 11, 13),
                    DateDue = new DateTime(2017, 11, 27),
                    ClientName = "Jason Carpets",
                    ClientContact = "297 Moorhouse Ave\\r\\nSydenham\\r\\nChristchurch 8011",
                    Status = InvoiceStatus.Sent,
                    GstRate = .15m,
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
                    InvoiceNumber = "20170909-001",
                    DateCreated = new DateTime(2017, 9, 9),
                    DateDue = new DateTime(2017, 9, 30),
                    ClientName = "Transtellar",
                    ClientContact = "52 Solmine Ave\\r\\nRiccarton\\r\\nChristchurch 8025",
                    Status = InvoiceStatus.Paid,
                    GstRate = .15m,
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
                    Email = "guest@guest.com",
                    Name = "guest",
                    Password = _cryptography.HashMD5("guest"),
                    Active = true
                };

                var user2 = new User()
                {
                    Email = "helersonlage@gmail.com",
                    Name = "Helerson Lage",
                    Password = "68eacb97d86f0c4621fa2b0e17cabd8c",
                    Active = true
                };

                var user3 = new User()
                {
                    Email = "j.george@cbanewzealand.org.nz",
                    Name = "John George",
                    Password = _cryptography.HashMD5("john"),
                    Active = true
                };

                _context.User.Add(user1);
                _context.User.Add(user2);
                _context.User.Add(user3);

                _context.SaveChanges();
            }
        }
    }
}

