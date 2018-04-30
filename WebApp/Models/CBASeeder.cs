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
                    DateDue = new DateTime(2017, 10, 17),
                    ClientName = "Electrocal Commission",
                    ClientContactPerson = "Glen Clarke",
                    ClientContact = "530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053",
                    Email = "ec@example.com",
                    PaymentId = _cryptography.GenerateTempPassword(12),
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
                    Email = "jc@example.com",
                    PaymentId = _cryptography.GenerateTempPassword(12),
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
                    Email = "t@example.com",
                    PaymentId = _cryptography.GenerateTempPassword(12),
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

                //Invoice Type is Draft
                //GST Type Exclusive 
                //GST Amount = 3.86M
                //Grand Total = 29.60
                var invoice4 = new Invoice()
                {
                    InvoiceNumber = "DRAFT0000001",
                    DateCreated = new DateTime(2018, 4, 16),
                    DateDue = new DateTime(2018, 5, 16),
                    ClientName = "ANZ",
                    ClientContact = "52 Solmine Ave\\r\\nRiccarton\\r\\nChristchurch 8025",
                    Email = "anz@example.com",

                    Status = InvoiceStatus.Draft,
                    GstRate = .15m,
                    GstNumber = "96-712-571",
                    CharitiesNumber = "CC20090",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Donation",
                            Amount = 25.74M
                        }
                    }
                };

                //Invoice Type is Sent
                //GST Type Exclusive
                //GST Amount = 3.11M
                //Grand Total = 23.86M
                var invoice5 = new Invoice()
                {
                    InvoiceNumber = "CBANZ00001/18",
                    DateCreated = new DateTime(2018, 3, 1),
                    DateDue = new DateTime(2018, 4, 15),
                    ClientName = "Supercare",
                    ClientContact = "5/92 Owairaka Avenue\\r\\nMt Albert\\r\\nAuckland 1025",
                    Email = "supercare@example.com",
                    PaymentId = _cryptography.GenerateTempPassword(12),
                    Status = InvoiceStatus.Sent,
                    GstRate = .15m,
                    GstNumber = "96-712-562",
                    CharitiesNumber = "CC20098",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Expenditure",
                            Amount = 20.75M
                        }
                    }
                };

                //Invoice Type is Paid
                //GST Type Inclusive
                //GST Amount = 6.52M
                //Grand Total = 50.00M
            
                var invoice6 = new Invoice()
                {
                    InvoiceNumber = "CBANZ00002/18",
                    DateCreated = new DateTime(2018, 3, 10),
                    DateDue = new DateTime(2018, 4, 1),
                    ClientName = "AT&T",
                    ClientContact = "52 St Lukes Road\\r\\nAuckland 1030",
                    Email = "at&t@example.com",
                    PaymentId = _cryptography.GenerateTempPassword(12),
                    Status = InvoiceStatus.Paid,
                    GstRate = .15m,
                    GstNumber = "96-712-563",
                    CharitiesNumber = "CC20099",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Fundraising Dinner",
                            Amount = 25.00M
                        },
                        new InvoiceLine()
                        {
                            Description = "Bookkeeping 2 hours @21.74 per hour",
                            Amount = 50.00M
                        }
                    }
                };

                //Invoice Type is Draft
                //GST Type Inclusive 
                //GST Amount = 3.21M
                //Grand Total = 24.60M
                var invoice7 = new Invoice()
                {
                    InvoiceNumber = "DRAFT0000002",
                    DateCreated = new DateTime(2018, 4, 25),
                    DateDue = new DateTime(2018, 6, 1),
                    ClientName = "IBM",
                    ClientContact = "5 Gaunt Street\\r\\nAuckland 1070",
                    Email = "ibm@example.com",

                    Status = InvoiceStatus.Draft,
                    GstRate = .15m,
                    GstNumber = "96-712-564",
                    CharitiesNumber = "CC20080",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Donation",
                            Amount = 24.60M
                        }
                    }
                };

                //Invoice type is Draft
                //GST type is inclusive
                //GST amount: 3M
                //Grand total:23M
                var invoice8 = new Invoice()
                {
                    InvoiceNumber = "DRAFT0000003",
                    DateCreated = new DateTime(2018, 3, 25),
                    DateDue = new DateTime(2018, 7, 10),
                    ClientName = "Concentrix",
                    ClientContact = "83 Carrington Rd, Mt Albert, Auckland 1025",
                    Email = "concentrix@example.com",
                    Status = InvoiceStatus.Draft,
                    GstRate = .15m,
                    GstNumber = "96-712-572",
                    CharitiesNumber = "CC20093",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Charity Dinner",
                            Amount = 23M
                        }
                    }
                };

                //Invoice type is sent
                //GST is inclusive
                //GST amount: 1.5M
                //Grand total:11.5M
                var invoice9 = new Invoice()
                {
                    InvoiceNumber = "CBANZ00003/18",
                    DateCreated = new DateTime(2018, 4, 29),
                    DateDue = new DateTime(2018, 6, 10),
                    ClientName = "Auckland Council",
                    ClientContact = "23 Quay St, Auckland 1050",
                    Email = "auckcouncil@example.com",
                    Status = InvoiceStatus.Sent,
                    GstRate = .15m,
                    GstNumber = "96-712-567",
                    CharitiesNumber = "CC20110",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Donation",
                            Amount = 11.5M
                        }
                    }
                };

                //Invoice type is Paid
                //GST is exclusive
                //GST amount: 0.86M
                //Grand total: 6.61M
                var invoice10 = new Invoice()
                {
                    InvoiceNumber = "CBANZ00004/18",
                    DateCreated = new DateTime(2018, 2, 20),
                    DateDue = new DateTime(2018, 4, 10),
                    ClientName = "Mike Wazowski",
                    ClientContact = "225, Great North Rd, Auckland 1024",
                    Email = "wazmike@example.com",
                    Status = InvoiceStatus.Paid,
                    GstRate = .15m,
                    GstNumber = "96-712-569",
                    CharitiesNumber = "CC20100",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Donation",
                            Amount =5.75M
                        }
                    }
                };

                //Invoice type is Draft
                //GST is exclusive
                //GST Amount: 4.31M
                //Grand total: 33.06M 
                var invoice11 = new Invoice()
                {
                    InvoiceNumber = "DRAFT0000004",
                    DateCreated = new DateTime(2018, 4, 24),
                    DateDue = new DateTime(2018, 10, 05),
                    ClientName = "Mo Salah",
                    ClientContact = "2a, Kingsway Ave, Auckland 1025",
                    Email = "m.salah@example.com",
                    Status = InvoiceStatus.Draft,
                    GstRate = .15m,
                    GstNumber = "96-712-566",
                    CharitiesNumber = "CC20101",
                    InvoiceLine = new List<InvoiceLine>()
                    {
                        new InvoiceLine()
                        {
                            Description = "Donation",
                            Amount = 28.75M
                        }
                    }
                };

                _context.Add(invoice1);
                _context.Add(invoice2);
                _context.Add(invoice3);
                _context.Add(invoice4);
                _context.Add(invoice5);
                _context.Add(invoice6);
                _context.Add(invoice7);
                _context.Add(invoice8);
                _context.Add(invoice9);
                _context.Add(invoice10);
                _context.Add(invoice11);
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

