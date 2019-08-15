using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MiniCMS.Infrastructure.Data;
using MiniCMS.Infrastructure.Repositories;
using MiniCMS.Infrastructure.Services;
using MiniCMS.Domain.Entities;
using MiniCMS.Api.Hubs;
using MiniCMS.Api.GraphQL;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace MiniCMS.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<MiniCmsDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IRepository<App>, AppRepository>();
            services.AddScoped<AppRepository>();
            services.AddScoped<SchemaRepository>();
            services.AddScoped<ContentRepository>();
            services.AddScoped<WebhookRepository>();
            services.AddScoped<AuditLogRepository>();

            // Services
            services.AddSingleton<IFileStorageService>(sp =>
                new LocalFileStorageService(Configuration["Storage:Path"] ?? "./uploads"));

            // Controllers
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddSignalR();

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .Create());

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MiniCMS API",
                    Version = "v1",
                    Description = "A lightweight headless CMS API",
                    Contact = new OpenApiContact
                    {
                        Name = "Thanh Vu",
                        Email = "thanhauco@gmail.com"
                    }
                });
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniCMS API v1");
                c.RoutePrefix = "swagger";
            });

            app.UseRouting();
            app.UseCors();
            app.UseResponseCaching();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ContentHub>("/hubs/content");
                endpoints.MapGraphQL("/api/graphql");
            });
        }
    }
}
