using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NUnit.Framework;
using ProductWebAPI.Controllers;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;
using System.Diagnostics.Metrics;

namespace WebApiUnitTest
{
    public class TransactionControllerTests
    {
        private ProductContext _dbContext;
        private TransactionController _transactionController;
        private List<Transaction> _transactions = new List<Transaction>()
        {
            new Transaction
            {
                Id = 1, ProductCode = "0001", ProductName = "Laptop", Price =4200, Quantity = 800,
            },
            new Transaction
            {
                Id = 2, ProductCode = "0002", ProductName = "MobilePhone", Price = 1100, Quantity = 1500,
            },
            new Transaction
            {
                Id = 3, ProductCode = "0003", ProductName = "Headset", Price = 80, Quantity = 35000,
            },
            new Transaction
            {
                Id = 4, ProductCode = "0002", ProductName = "MobilePhone", Price = 1000, Quantity = 2500,
            }
        };

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
             .UseInMemoryDatabase(databaseName: "ProductTestDatabase").Options;

            _dbContext = new ProductContext(options);

            _transactionController = new TransactionController(_dbContext);
        }

        [Test]
        public void GetByIdTest()
        {
            _transactionController.SetTransactions(_transactions);

            var transaction = _transactionController.GetById(4);

            Assert.IsNotNull(transaction);
            Assert.AreEqual(4, transaction.Id);
            Assert.AreEqual("0002", transaction.ProductCode);
            Assert.AreEqual("MobilePhone", transaction.ProductName);
            Assert.AreEqual(1000, transaction.Price);
            Assert.AreEqual(2500, transaction.Quantity);
        }

        [Test]
        public void AddTest()
        {
            var count = _transactionController.GetTransactions().Count();

            _transactionController.Add(_transactions[0]);

            var count1 = _transactionController.GetTransactions().Count();

            Assert.AreEqual(count + 1, count1);
        }

        [Test]
        public void UpdateTest()
        {
            _transactionController.SetTransactions(_transactions);
            var transaction = new Transaction
            {
                Id = 2,
                ProductCode = "002",
                ProductName = "Toy",
                Price = 50,
                Quantity = 95000
            };

            _transactionController.update(transaction);

            var updatedTransaction = _transactionController.GetById(transaction.Id);

            Assert.IsNotNull(updatedTransaction);
            Assert.AreEqual("002", updatedTransaction.ProductCode);
            Assert.AreEqual("Toy", updatedTransaction.ProductName);
            Assert.AreEqual(50, updatedTransaction.Price);
            Assert.AreEqual(95000, updatedTransaction.Quantity);
        }

        [Test]
        public void DeleteTest()
        {
            _transactionController.SetTransactions(_transactions);

            var count = _transactionController.GetTransactions().Count();

            _transactionController.delete(3);

            var count1 = _transactionController.GetTransactions().Count();

            Assert.AreEqual(count - 1, count1);
        }

        [Test]
        public void CanGetProductSalesForWeekly()
        {
            _transactionController.SetTransactions(_transactions);
            var weeklyProductSales = _transactionController.GetProductSalesForWeekly().Result.ToList();

            Verify(weeklyProductSales);
        }

        [Test]
        public void CanGetProductSalesForMonthly()
        {
            _transactionController.SetTransactions(_transactions);
            var monthlyProductSales = _transactionController.GetProductSalesForMonthly().Result.ToList();

            Verify(monthlyProductSales);
        }

        private void Verify(List<ProductSale> productSale)
        {
            Assert.IsNotNull(productSale);
            Assert.That(productSale.Count == 3);

            Assert.AreEqual("0001", productSale[0].ProductCode);
            Assert.AreEqual("Laptop", productSale[0].ProductName);
            Assert.AreEqual(3360000, productSale[0].GrossSales);
            Assert.AreEqual(800, productSale[0].TotalQuantity);


            Assert.AreEqual("0002", productSale[1].ProductCode);
            Assert.AreEqual("MobilePhone", productSale[1].ProductName);
            Assert.AreEqual(4150000, productSale[1].GrossSales);
            Assert.AreEqual(4000, productSale[1].TotalQuantity);


            Assert.AreEqual("0003", productSale[2].ProductCode);
            Assert.AreEqual("Headset", productSale[2].ProductName);
            Assert.AreEqual(2800000, productSale[2].GrossSales);
            Assert.AreEqual(35000, productSale[2].TotalQuantity);

        }
    }
}