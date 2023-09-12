using EManEmailMarketing.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EManEmailMarketing.Storage
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Emails> Emails { get; set; }

        public virtual DbSet<ScrapedEmail> ScrapedEmails { get; set; }

        public virtual DbSet<ScrapedWebsite> ScrapedWebsites { get; set; }

        public virtual DbSet<ClientProgress> ClientsProgress { get; set; }

        public virtual DbSet<SentEmail> SentEmails { get; set;}

        public virtual DbSet<KeyWord> KeyWords { get; set; }

        public virtual DbSet<ScrapedEmailLocation> ScrapedEmailLocations { get; set; }

        public virtual DbSet<EmailCallback> EmailCallbacks { get; set; }
    }
}
