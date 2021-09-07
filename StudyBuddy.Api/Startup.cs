using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

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

            services.AddControllers();
            services.AddSingleton<IRepository, Repository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "StudyBuddy API",
                    Description = "The RESful API of StudyBuddy",
                    Version = "v1"
                });
            });

            var settings = GetMailKitOptions();
            services.AddMailKit(optionBuilder => { optionBuilder.UseMailKit(settings); });
        }

        private MailKitOptions GetMailKitOptions()
        {
            var options = new MailKitOptions();
            options.Server = Helper.GetFromEnvironmentOrDefault("SMTP_SERVER", "localhost");
            options.Port = Convert.ToInt32(Helper.GetFromEnvironmentOrDefault("SMTP_PORT", "587"));
            options.SenderName = Helper.GetFromEnvironmentOrDefault("SMTP_SENDERNAME", "admin");
            options.SenderEmail = Helper.GetFromEnvironmentOrDefault("SMTP_SENDEREMAIL", "admin@admin.de");
            options.Account = Helper.GetFromEnvironmentOrDefault("SMTP_ACCOUNT", "admin@admin.de");
            options.Password = Helper.GetFromEnvironmentOrDefault("SMTP_PASSWORD", "secret");
            return options;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("all");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyBuddy API");
                options.RoutePrefix = string.Empty;
            });

            app.UseCustomAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
