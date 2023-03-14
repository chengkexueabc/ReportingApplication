using ReportingApplication.Model;
using System.Collections.Generic;

namespace ReportingApplication.Rest
{
    public interface IRestTemplate
    {
        public List<ProductSale> GetWeeklyReport(string url);

        public List<ProductSale> GetMonthlyReport(string url);

        public List<ShippingDestination> GetShippingDestinationReport(string url);
    }
}
