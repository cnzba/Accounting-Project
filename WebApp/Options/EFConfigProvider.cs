using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.Options
{
    public class EFConfigProvider : ConfigurationProvider
    {
        public EFConfigProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        Action<DbContextOptionsBuilder> OptionsAction { get; }

        // Load config data from EF DB.
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<CBAOptionsContext>();
            OptionsAction(builder);

            using (var dbContext = new CBAOptionsContext(builder.Options))
            {
                dbContext.Database.EnsureCreated(); 

                Data = !dbContext.ConfigurationValue.Any()
                    ? CreateAndSaveDefaultValues(dbContext)
                    : dbContext.ConfigurationValue.ToDictionary(c => c.Id, c => c.Value);
            }
        }

        private static IDictionary<string, string> CreateAndSaveDefaultValues(
            CBAOptionsContext dbContext)
        {
            var configValues = new Dictionary<string, string>
                {
                    { "GSTRate", ".15" },
                    { "CharitiesNumber", "CC20097" },
                    { "GSTNumber", "96-712-561" }
                };
            dbContext.ConfigurationValue.AddRange(configValues
                .Select(kvp => new ConfigurationValue { Id = kvp.Key, Value = kvp.Value })
                .ToArray());
            dbContext.SaveChanges();
            return configValues;
        }
    }
}
