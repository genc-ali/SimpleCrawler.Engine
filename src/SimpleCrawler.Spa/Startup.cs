using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SimpleCrawler.Core.Database;
using SimpleCrawler.Core.MessageQueue.RabbitMq;
using SimpleCrawler.Domain;
using SimpleCrawler.MongoDb;
using SimpleCrawler.MongoDb.Repository;
using SimpleCrawler.SinglePageApp.Infrastructure;
using SimpleCrawler.SinglePageApp.Infrastructure.Crawlers;
using SimpleCrawler.SinglePageApp.Infrastructure.MessageQueue;

namespace SimpleCrawler.SinglePageApp
{
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
            services.AddSingleton(_ => (IConfigurationRoot) Configuration);
            services.AddSingleton<AppConfiguration>();
            services.AddScoped<IDbContext, SimpleCrawlerDbContext>();
            
            services.AddScoped<IQueryKeywordRepository, QueryKeywordRepository>();
            services.AddScoped<ApplicationService>();
            services.AddScoped<IApplicationAdapter, ApplicationAdapter>();
            
            services.AddHostedService<KeywordSearchBackgroundService>();
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<GoogleCrawler>();
            
            services.AddControllersWithViews();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "QueryKeyword API",
                    Description = "Keyword search engine.",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}