using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;
using System.Collections.Generic;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ProductContext _context;
        private List<Transaction> _transactions;


        public TransactionController(ProductContext context)
        {
            _context = context;
            _transactions = _context.Transactions.ToList();
        }

        [HttpGet]
        public IEnumerable<Transaction> GetTransactions()
        {
            return _transactions;
        }

        [HttpGet]
        [Route("WeeklyReport")]
        public async Task<IEnumerable<ProductSale>> GetProductSalesForWeekly()
        {
            var productSalesForWeekly = _transactions.GroupBy(n => new
            {
                productCode = n.ProductCode,
                productName = n.ProductName
            }).Select(
                        n => new ProductSale
                        {
                            ProductCode = n.Key.productCode,
                            ProductName = n.Key.productName,
                            GrossSales = n.Sum(n2 => n2.Quantity * n2.Price),
                            TotalQuantity = n.Sum(n2 => n2.Quantity)
                        }
                      ).OrderBy(x => x.ProductCode);

            return productSalesForWeekly;
        }

        [HttpGet]
        [Route("MonthlyReport")]
        public async Task<IEnumerable<ProductSale>> GetProductSalesForMonthly()
        {
            var productSalesForMonthly = _transactions.GroupBy(n => new
            {
                productCode = n.ProductCode,
                productName = n.ProductName
            }).Select(
                        n => new ProductSale
                        {
                            ProductCode = n.Key.productCode,
                            ProductName = n.Key.productName,
                            GrossSales = n.Sum(n2 => n2.Quantity * n2.Price),
                            TotalQuantity = n.Sum(n2 => n2.Quantity)
                        }
                      ).OrderBy(x => x.ProductCode);

            return productSalesForMonthly;
        }

        [HttpGet]
        [Route("get")]
        public Transaction GetById(long id)
        {
            var transaction = _transactions.Where(x => x.Id == id).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Can't get a non-existing transaction");
            }
            return transaction;
        }

        [HttpPost]
        [Route("settransactions")]
        public void SetTransactions(List<Transaction> transactions)
        {
            _transactions = transactions;
        }

        [HttpPost]
        [Route("add")]
        public void Add(Transaction transaction)
        {
            if (_transactions.Count(x =>x.Id == transaction.Id) > 0) 
            {
                throw new Exception("Can't add a existing transaction!");
            }
            _transactions.Add(transaction);
        }

        [HttpPost]
        [Route("update")]
        public void update(Transaction transaction)
        {
            var t = _transactions.Where(x => x.Id == transaction.Id).FirstOrDefault();
            if (t == null)
            {
                throw new Exception("Can't update a non-existing transaction");
            }
            _transactions.ForEach(x =>
            {
                if (x.Id == transaction.Id)
                {
                    x.ProductCode = transaction.ProductCode;
                    x.ProductName = transaction.ProductName;
                    x.Price = transaction.Price;
                    x.Quantity= transaction.Quantity;
                }
            });
        }

        [HttpPost]
        [Route("delete")]
        public void delete(long id)
        {
            var transaction = _transactions.Where(x => x.Id == id).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Can't delete a non-existing transaction");
            }
            _transactions.Remove(transaction);
        }

    }
}
