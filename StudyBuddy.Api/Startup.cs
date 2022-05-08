using System;
using Cronos;
using EasyCronJob.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StudyBuddy.BusinessLogic;
using StudyBuddy.BusinessLogic.Cron;

namespace StudyBuddy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("all",
                policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            ));

            services.AddControllers(options => { options.Filters.Add(typeof(JsonExceptionFilter)); });

            services.AddScoped<IBackend, Backend>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "StudyBuddy API",
                    Description = "The RESful API of StudyBuddy",
                    Version = "v1"
                });
            });

            services.ApplyResulation<UnseenNotificationsCron>(options =>
            {
                options.CronExpression = "0 7 * * MON";
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronFormat = CronFormat.Standard;
            });

            services.ApplyResulation<DeleteOldFCMTokensCron>(options =>
            {
                options.CronExpression = "0 4 * * *";
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronFormat = CronFormat.Standard;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("all");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyBuddy API");
                options.RoutePrefix = string.Empty;
            });

            app.UseCustomAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}