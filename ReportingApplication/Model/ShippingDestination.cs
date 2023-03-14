namespace ReportingApplication.Model
{
    public class ShippingDestination
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Destination { get; set; }
        public int TotalQuantity { get; set; }
        public double AverageShippingTime { get; set; }
    }
}
