using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using ReportingApplication.Email;
using ReportingApplication.Rest;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;
using ReportingApplication.Model;
using ReportingApplication.Html;
using ReportingApplication.EncryptUtils;

namespace ReportingApplication.Job
{
    public class ShippingDestinationJob : IJob
    {
        private readonly ILogger<ShippingDestinationJob> _logger;
        private readonly IRestTemplate _restTemplate;
        private readonly IEmailService _emailSend;
        private readonly IHtmlGenerate _htmlGenerate;
        private readonly IConfiguration _configuration;

        public ShippingDestinationJob(ILogger<ShippingDestinationJob> logger, IRestTemplate restTemplate, IEmailService emailSend,
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
            string url = $"{apiUrl}/Shipping/Destination";
            var shippings = _restTemplate.GetShippingDestinationReport(url);

            var month = DateTime.Now.AddMonths(-1).ToString("yyyyMM");

            var directory = AppContext.BaseDirectory + "shipping destination " + month;
            var fileName = directory + ".html";
            _htmlGenerate.CreateShippingDestinationHtml(fileName, shippings);

            var email = new Model.Email();
            email.AddFrom = _configuration["Email:Smtp:From"];
            email.AddTo = _configuration["Email:Smtp:WeeklyTo"];
            email.Host = _configuration["Email:Smtp:Host"];
            email.Port = Convert.ToInt32(_configuration["Email:Smtp:Port"]);
            email.Password = Base64Helper.Base64Decrypt(_configuration["Email:Smtp:Password"]);
            email.Subject = "Products Shipping Destination Distribution Report";
            var header = $"<strong><b>This is products shipping destination distribution report(month:{month})</b></strong><br/><br/>";

            email.Body = header + BuildBody(shippings);

            email.AttachmentFile = fileName;
            _emailSend.SendEmail(email);

            return Task.CompletedTask;
        }

        private string BuildBody(List<ShippingDestination> shippings)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table border=\"1\">");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Product Code</th>");
            sb.AppendLine("<th>Product Name</th>");
            sb.AppendLine("<th>Destination</th>");
            sb.AppendLine("<th>Total Quantity</th>");
            sb.AppendLine("<th>Average Shipping Time</th>");
            sb.AppendLine("</tr>");
            foreach (var item in shippings)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + item.ProductCode + "</td>");
                sb.AppendLine("<td>" + item.ProductName + "</td>");
                sb.AppendLine("<td>" + item.Destination + "</td>");
                sb.AppendLine("<td>" + item.TotalQuantity + "</td>");
                sb.AppendLine("<td>" + item.AverageShippingTime + "</td>");
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
