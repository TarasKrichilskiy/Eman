using AngleSharp;
using Azure.Storage.Queues;
using EManEmailMarketing.Common.Constants;
using EManEmailMarketing.Common.Telemetry;
using EManEmailMarketing.EmailingManager.Services;
using EManEmailMarketing.Scraper.Interfaces;
using EManEmailMarketing.Scraper.Services;
using EManEmailMarketing.SmartMail.Services;
using EManEmailMarketing.Storage;
using EManEmailMarketing.Storage.Repositories;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

[assembly: FunctionsStartup(typeof(EManEmailMarketing.Startup))]
namespace EManEmailMarketing
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            //    .AddEnvironmentVariables();
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var context = builder.GetContext();
            var connectionString = context.Configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection");
            var queueConnectionString = context.Configuration.GetSection("ConnectionStrings").GetValue<string>("QueueConnection");
            var optionsSection = context.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options");
            builder.Services.AddLogging(logging =>
            {
                logging.AddDatabaseLogger(options =>
                {
                    optionsSection.Bind(options);
                });
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            });
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            },
            ServiceLifetime.Transient);
            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddScoped<DbContext, ApplicationDbContext>();
            builder.Services.AddScoped<IEmailFrequencyDeterminer, EmailFrequencyDeterminer>();
            builder.Services.AddScoped<IMain, Main>();
            builder.Services.AddScoped<IWebsiteScraper, WebsiteScraper>();
            builder.Services.AddScoped<IClientProgressRepository, ClientProgressRepository>();
            builder.Services.AddScoped<ISentEmailsRepository, SentEmailsRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IScrapedEmailRepository, ScrapedEmailRepository>();
            builder.Services.AddScoped<IScrapedEmailLocationRepository, ScrapedEmailLocationRepository>();
            builder.Services.AddScoped<IScrapedWebsiteRepository, ScrapedWebsiteRepository>();
            builder.Services.AddScoped<IEmailCallbackRepository, EmailCallbackRepository>();
            builder.Services.AddScoped<IEmailQueueRepository>(_ => new EmailQueueRepository(new QueueClient(queueConnectionString, QueueConstants.EmailQueue)));
            builder.Services.AddScoped<IKeyWordRepository, KeyWordRepository>();

        }
    }
}
