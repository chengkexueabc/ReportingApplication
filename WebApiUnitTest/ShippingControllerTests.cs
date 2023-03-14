using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProductWebAPI.Controllers;
using ProductWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiUnitTest
{
    public class ShippingControllerTests
    {
        private ProductContext _dbContext;
        private ShippingController _shippingController;
        private List<Transaction> _transactions = new List<Transaction>()
        {
            new Transaction
            {
                Id = 101, ProductCode = "0001", ProductName = "Laptop", Price =4200, Quantity = 800,
            },
            new Transaction
            {
                Id = 102, ProductCode = "0002", ProductName = "MobilePhone", Price = 1100, Quantity = 1500,
            },
            new Transaction
            {
                Id = 103, ProductCode = "0003", ProductName = "Headset", Price = 80, Quantity = 35000,
            },
            new Transaction
            {
                Id = 104, ProductCode = "0002", ProductName = "MobilePhone", Price = 1000, Quantity = 2500,
            }
        };

        private List<Shipping> _shipping = new List<Shipping>()
        {
            new Shipping
            {
                Id = 101, ProductCode = "0001", ProductName = "Laptop", Destination = "BeiJing", ShippingTime = 50,
            },
            new Shipping
            {
                Id = 102, ProductCode = "0002", ProductName = "MobilePhone", Destination = "ShangHai", ShippingTime = 40,
            },
            new Shipping
            {
                Id = 103, ProductCode = "0003", ProductName = "Headset", Destination = "ShenZhen", ShippingTime = 25,
            },
            new Shipping
            {
                Id = 104, ProductCode = "0002", ProductName = "MobilePhone", Destination = "ShangHai", ShippingTime = 55,
            },
        };

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
             .UseInMemoryDatabase(databaseName: "ProductTestDatabase").Options;

            _dbContext = new ProductContext(options);

            _shippingController = new ShippingController(_dbContext);
        }

        [Test]
        public void CanGetShippingDestinations()
        {
            _shippingController.SetTransactions(_transactions);
            _shippingController.SetShippings(_shipping);
            var shippings = _shippingController.GetShippingDestinations().Result.ToList();

            Assert.IsNotNull(shippings);
            Assert.AreEqual(3, shippings.Count);

            Assert.AreEqual("0001", shippings[0].ProductCode);
            Assert.AreEqual("Laptop", shippings[0].ProductName);
            Assert.AreEqual("BeiJing", shippings[0].Destination);
            Assert.AreEqual(800, shippings[0].TotalQuantity);
            Assert.AreEqual(50, shippings[0].AverageShippingTime);


            Assert.AreEqual("0002", shippings[1].ProductCode);
            Assert.AreEqual("MobilePhone", shippings[1].ProductName);
            Assert.AreEqual("ShangHai", shippings[1].Destination);
            Assert.AreEqual(4000, shippings[1].TotalQuantity);
            Assert.AreEqual(47.5, shippings[1].AverageShippingTime);


            Assert.AreEqual("0003", shippings[2].ProductCode);
            Assert.AreEqual("Headset", shippings[2].ProductName);
            Assert.AreEqual("ShenZhen", shippings[2].Destination);
            Assert.AreEqual(35000, shippings[2].TotalQuantity);
            Assert.AreEqual(25, shippings[2].AverageShippingTime);
        }
    }
}
