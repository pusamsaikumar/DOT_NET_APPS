namespace RSASupportAPI.Models
{
    public class GetUserRecentPurchasedProducts
    {
        public string ProductName { get; set; } 
        public string ProductCode { get; set; }
        public int? Qty { get; set; }
        public decimal? Amount { get; set; }
    }
}
