using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EManEmailMarketing.Storage;
using EManEmailMarketing.EmailingManager.Services;
using EManEmailMarketing.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using EManEmailMarketing.Storage.Repositories.Interface;

namespace EManEmailMarketing.EmailingManager;


public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddDbContext<ApplicationDbContext>();
        services.AddScoped<DbContext, ApplicationDbContext>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IClientProgressRepository, ClientProgressRepository>();
        services.AddScoped<ISentEmailsRepository, SentEmailsRepository>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
