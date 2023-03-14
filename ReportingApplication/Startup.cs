using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using ReportingApplication.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportingApplication.MonitorService;
using System.Net.Mail;
using System.Net;
using ReportingApplication.Rest;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReportingApplication.Email;
using ReportingApplication.Html;

namespace ReportingApplication
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
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<WeeklySaleReportJob>();
            services.AddSingleton<MonthlySaleReportJob>();
            services.AddSingleton<ShippingDestinationJob>();
            services.AddSingleton(
                 new JobSchedule(jobType: typeof(WeeklySaleReportJob), cronExpression: Configuration["CronExpression:Weekly"])
            );

            services.AddSingleton(
                 new JobSchedule(jobType: typeof(MonthlySaleReportJob), cronExpression: Configuration["CronExpression:Monthly"])
            );

            services.AddSingleton(
                 new JobSchedule(jobType: typeof(ShippingDestinationJob), cronExpression: Configuration["CronExpression:Destination"])
            );

            Console.WriteLine(Configuration["CronExpression:Weekly"]);
            Console.WriteLine(Configuration["CronExpression:Monthly"]);
            Console.WriteLine(Configuration["CronExpression:Destination"]);

            services.AddHostedService<QuartzHostedService>();

            services.AddSingleton<IRestTemplate, RestTemplate>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IHtmlGenerate, HtmlGenerate>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
