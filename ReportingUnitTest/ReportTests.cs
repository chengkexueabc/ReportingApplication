using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ReportingApplication.Html;
using ReportingApplication.Model;
using ReportingApplication.Rest;

namespace ReportingApplicationTest
{
    public class ReportTests
    {
        private IRestTemplate _restTemplate;
        private IHtmlGenerate _htmlGenerate;
        private List<ProductSale> _productSales = new List<ProductSale> {
            new ProductSale
            {
                ProductCode = "0002", ProductName = "MobilePhone", GrossSales = 1650000, TotalQuantity = 1500
            },
            new ProductSale
            {
                ProductCode = "0001", ProductName = "Laptop", GrossSales =3360000, TotalQuantity = 800
            },
            new ProductSale
            {
                ProductCode = "0003", ProductName = "Headset", GrossSales = 2800000, TotalQuantity = 35000
            },
        };
        private List<ShippingDestination> _shippingDestination = new List<ShippingDestination> {
            new ShippingDestination
            {
                ProductCode = "0001", ProductName = "Laptop", Destination ="ShenZhen", TotalQuantity = 800, AverageShippingTime = 15
            },
            new ShippingDestination
            {
                ProductCode = "0002", ProductName = "MobilePhone", Destination ="DongGuan", TotalQuantity = 1500, AverageShippingTime = 20
            }
        };

        [SetUp]
        public void Setup()
        {
            Mock<IRestTemplate> mockObject = new Mock<IRestTemplate>();

            mockObject.Setup(x => x.GetWeeklyReport("Weekly")).Returns(_productSales);
            mockObject.Setup(x => x.GetMonthlyReport("Monthly")).Returns(_productSales);
            mockObject.Setup(x => x.GetShippingDestinationReport("ShippingDestination")).Returns(_shippingDestination);
            _restTemplate = mockObject.Object;

            LoggerFactory factory = new LoggerFactory();
            ILogger<HtmlGenerate> logger = new Logger<HtmlGenerate>(factory);
            _htmlGenerate = new HtmlGenerate(logger);

        }


        [Test]
        public void WeeklyReportTest()
        {
            var productSales = _restTemplate.GetWeeklyReport("Weekly");

            Assert.IsNotNull(productSales);
            Assert.AreEqual(3, productSales.Count);

            var directory = AppContext.BaseDirectory + "Weekly" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = directory + ".html";
            _htmlGenerate.CreateProductSaleHtml(fileName, productSales);

            Assert.IsTrue(File.Exists(fileName));

            var sr = new StreamReader(fileName);
            string context = sr.ReadToEnd();
            sr.Close();

            Assert.IsTrue(context.Contains(productSales[0].ProductName));
            Assert.IsTrue(context.Contains(productSales[1].ProductName));
            Assert.IsTrue(context.Contains(productSales[2].ProductName));

            File.Delete(fileName);
        }

        [Test]
        public void MonthlyReportTest()
        {
            var productSales = _restTemplate.GetMonthlyReport("Monthly");

            Assert.IsNotNull(productSales);
            Assert.AreEqual(3, productSales.Count);

            var directory = AppContext.BaseDirectory + "Monthly" +DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = directory + ".html";
            _htmlGenerate.CreateProductSaleHtml(fileName, productSales);

            Assert.IsTrue(File.Exists(fileName));

            var sr = new StreamReader(fileName);
            string context = sr.ReadToEnd();
            sr.Close();

            Assert.IsTrue(context.Contains(productSales[0].ProductName));
            Assert.IsTrue(context.Contains(productSales[1].ProductName));
            Assert.IsTrue(context.Contains(productSales[2].ProductName));

            File.Delete(fileName);
        }

        [Test]
        public void ShippingDestinationReportTest()
        {
            var shippingDestination = _restTemplate.GetShippingDestinationReport("ShippingDestination");

            Assert.IsNotNull(shippingDestination);
            Assert.AreEqual(2, shippingDestination.Count);

            var directory = AppContext.BaseDirectory + "ShippingDestination" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = directory + ".html";
            _htmlGenerate.CreateShippingDestinationHtml(fileName, shippingDestination);

            Assert.IsTrue(File.Exists(fileName));

            var sr = new StreamReader(fileName);
            string context = sr.ReadToEnd();
            sr.Close();

            Assert.IsTrue(context.Contains(shippingDestination[0].ProductName));
            Assert.IsTrue(context.Contains(shippingDestination[1].ProductName));

            File.Delete(fileName);
        }
    }
}