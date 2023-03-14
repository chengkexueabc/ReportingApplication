using ReportingApplication.Model;
using System.Collections.Generic;

namespace ReportingApplication.Html
{
    public interface IHtmlGenerate
    {
        public void CreateProductSaleHtml(string fileName, List<ProductSale> productSale);
        public void CreateShippingDestinationHtml(string fileName, List<ShippingDestination> shippingDestinations);
    }
}
