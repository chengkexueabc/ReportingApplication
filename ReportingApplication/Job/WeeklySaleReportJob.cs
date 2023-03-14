using Microsoft.Extensions.Logging;
using Quartz;
using ReportingApplication.Email;
using System.Text;
using System.Threading.Tasks;
using System;
using ReportingApplication.Model;
using ReportingApplication.Rest;
using Microsoft.Extensions.Configuration;
using FluentEmail.Core;
using System.Collections.Generic;
using ReportingApplication.Html;
using ReportingApplication.EncryptUtils;

namespace ReportingApplication.Job
{
    public class WeeklySaleReportJob : IJob
    {
        private readonly ILogger<WeeklySaleReportJob> _logger;
        private readonly IRestTemplate _restTemplate;
        private readonly IEmailService _emailSend;
        private readonly IHtmlGenerate _htmlGenerate;
        private readonly IConfiguration _configuration;

        public WeeklySaleReportJob(ILogger<WeeklySaleReportJob> logger, IRestTemplate restTemplate, IEmailService emailSend,
            IHtmlGenerate htmlGenerate, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _restTemplate = restTemplate;
            _emailSend = emailSend;
            _htmlGenerate = htmlGenerate;
            _configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string apiUrl = _configuration["WebApiUrl"];
            string url = $"{apiUrl}/Transaction/WeeklyReport";
            var productSales = _restTemplate.GetWeeklyReport(url);

            var week = Convert.ToInt32(DateTime.Now.DayOfWeek);
            week = week == 0 ? 7 : week;
            var start = DateTime.Now.AddDays(1 - week).ToString("yyyyMMdd");
            var end = DateTime.Now.AddDays(7 - week).ToString("yyyyMMdd");

            var directory = AppContext.BaseDirectory + start + "_" + end;
            var fileName = directory + ".html";
            _htmlGenerate.CreateProductSaleHtml(fileName, productSales);

            var email = new Model.Email();
            email.AddFrom = _configuration["Email:Smtp:From"];
            email.AddTo = _configuration["Email:Smtp:WeeklyTo"];
            email.Host = _configuration["Email:Smtp:Host"];
            email.Port = Convert.ToInt32(_configuration["Email:Smtp:Port"]);
            email.Password = Base64Helper.Base64Decrypt(_configuration["Email:Smtp:Password"]);
            email.Subject = "Weekly Products Sale Report";
            var header = $"<strong><b>This is weekly products sale report({start}_{end})</b></strong><br/><br/>";

            email.Body = header + BuildBody(productSales);

            email.AttachmentFile = fileName;
            _emailSend.SendEmail(email);

            return Task.CompletedTask;
        }

        private string BuildBody(List<ProductSale> ProductSales)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table border=\"1\">");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Product Code</th>");
            sb.AppendLine("<th>Product Name</th>");
            sb.AppendLine("<th>Gross Sales</th>");
            sb.AppendLine("<th>Total Quantity</th>");
            sb.AppendLine("</tr>");
            foreach (var item in ProductSales)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + item.ProductCode + "</td>");
                sb.AppendLine("<td>" + item.ProductName + "</td>");
                sb.AppendLine("<td>" + item.GrossSales + "</td>");
                sb.AppendLine("<td>" + item.TotalQuantity + "</td>");
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}

