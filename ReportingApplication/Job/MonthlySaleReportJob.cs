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
    public class MonthlySaleReportJob : IJob
    {
        private readonly ILogger<MonthlySaleReportJob> _logger;
        private readonly IRestTemplate _restTemplate;
        private readonly IEmailService _emailSend;
        private readonly IHtmlGenerate _htmlGenerate;
        private readonly IConfiguration _configuration;

        public MonthlySaleReportJob(ILogger<MonthlySaleReportJob> logger, IRestTemplate restTemplate, IEmailService emailSend,
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
            string url = $"{apiUrl}/Transaction/MonthlyReport";
            var productSales = _restTemplate.GetMonthlyReport(url);

            var month = DateTime.Now.AddMonths(-1).ToString("yyyyMM");

            var directory = AppContext.BaseDirectory + "products sale " + month;
            var fileName = directory + ".html";
            _htmlGenerate.CreateProductSaleHtml(fileName, productSales);

            var email = new Model.Email();
            email.AddFrom = _configuration["Email:Smtp:From"];
            email.AddTo = _configuration["Email:Smtp:WeeklyTo"];
            email.Host = _configuration["Email:Smtp:Host"];
            email.Port = Convert.ToInt32(_configuration["Email:Smtp:Port"]);
            email.Password = Base64Helper.Base64Decrypt(_configuration["Email:Smtp:Password"]);
            email.Subject = "Monthly Products Sale Report";
            var header = $"<strong><b>This is monthlhy products sale report(month:{month})</b></strong><br/><br/>";

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