using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;
using System.Linq;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly ProductContext _context;
        private List<Transaction> _transactions;
        private List<Shipping> _shippings;

        public ShippingController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Destination")]
        public async Task<IEnumerable<ShippingDestination>> GetShippingDestinations()
        {
            _transactions = await _context.Transactions.ToListAsync();
            _shippings = await _context.Shippings.ToListAsync();

            var shippingDestinations = new List<ShippingDestination>();
            _shippings.ForEach(x =>
            {
                shippingDestinations.Add(
                    new ShippingDestination { 
                        ProductCode = x.ProductCode, 
                        ProductName = x.ProductName,
                        Destination = x.Destination,
                        AverageShippingTime = x.ShippingTime
                    });
            });

            var numofThread = new ParallelOptions { MaxDegreeOfParallelism = 4 };
            Parallel.ForEach(shippingDestinations, numofThread, SetTotalQuantity);

            var results = shippingDestinations.GroupBy(n => new
            {
                productCode = n.ProductCode,
                productName = n.ProductName,
                destination = n.Destination
            }).Select(n => new ShippingDestination
            {
                ProductCode = n.Key.productCode,
                ProductName = n.Key.productName,
                Destination = n.Key.destination,
                TotalQuantity = n.Sum(x => x.TotalQuantity) / n.Count(),
                AverageShippingTime = n.Sum(x => x.AverageShippingTime) / n.Count()
            }
            ).OrderBy(x => x.ProductCode);

            return results;
        }

        [HttpPost]
        [Route("settransactions")]
        public void SetTransactions(List<Transaction> transactions)
        {
            _context.Transactions.AddRange(transactions);
            _context.SaveChanges();
        }

        [HttpPost]
        [Route("setshippings")]
        public void SetShippings(List<Shipping> shippings)
        {
            _context.Shippings.AddRange(shippings);
            _context.SaveChanges();
        }

        private void SetTotalQuantity(ShippingDestination shippingDestination)
        {
            shippingDestination.TotalQuantity = _transactions.Where(x => x.ProductCode == shippingDestination.ProductCode)
                .Sum(x => x.Quantity);
        }
    }
}
