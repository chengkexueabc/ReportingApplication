using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Extensions.Logging;
using ReportingApplication.Email;
using ReportingApplication.Model;

namespace ReportingApplication.Html
{
    public class HtmlGenerate : IHtmlGenerate
    {
        private readonly ILogger<HtmlGenerate> _logger;
        public HtmlGenerate(ILogger<HtmlGenerate> logger)
        {
            _logger = logger;
        }
        public void CreateProductSaleHtml(string fileName, List<ProductSale> productSale)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    File.Create(fileName);
                }
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.WriteLine("<table border=\"1\">");
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<th>Product Code</th>");
                    sw.WriteLine("<th>Product Name</th>");
                    sw.WriteLine("<th>Gross Sales</th>");
                    sw.WriteLine("<th>Total Quantity</th>");
                    sw.WriteLine("</tr>");
                    foreach (ProductSale item in productSale)
                    {
                        sw.WriteLine("<tr>");
                        sw.WriteLine("<td>" + item.ProductCode + "</td>");
                        sw.WriteLine("<td>" + item.ProductName + "</td>");
                        sw.WriteLine("<td>" + item.GrossSales + "</td>");
                        sw.WriteLine("<td>" + item.TotalQuantity + "</td>");
                        sw.WriteLine("</tr>");
                    }

                    sw.WriteLine("</table>");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("create product sale html failed due to {0}", ex.ToString());
            }
        }

        public void CreateShippingDestinationHtml(string fileName, List<ShippingDestination> shippingDestinations)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.WriteLine("<table border=\"1\">");
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<th>Product Code</th>");
                    sw.WriteLine("<th>Product Name</th>");
                    sw.WriteLine("<th>Destination</th>");
                    sw.WriteLine("<th>Total Quantity</th>");
                    sw.WriteLine("<th>Average Shipping Time</th>");
                    sw.WriteLine("</tr>");
                    foreach (ShippingDestination item in shippingDestinations)
                    {
                        sw.WriteLine("<tr>");
                        sw.WriteLine("<td>" + item.ProductCode + "</td>");
                        sw.WriteLine("<td>" + item.ProductName + "</td>");
                        sw.WriteLine("<td>" + item.Destination + "</td>");
                        sw.WriteLine("<td>" + item.TotalQuantity + "</td>");
                        sw.WriteLine("<td>" + item.AverageShippingTime + "</td>");
                        sw.WriteLine("</tr>");
                    }

                    sw.WriteLine("</table>");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("create shipping destination html failed due to {0}", ex.ToString());
            }
        }
    }
}
