namespace ProductWebAPI.Models
{
    public class Shipping
    {
        public long Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Destination { get; set; }
        public int ShippingTime { get; set; }
    }
}
