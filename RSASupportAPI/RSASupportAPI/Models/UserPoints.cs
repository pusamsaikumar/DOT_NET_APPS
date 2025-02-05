namespace RSASupportAPI.Models
{
    public class UserPoints
    {
        public string? MemberNumber { get; set; }
        public string UPC1 { get; set; }
        public string UPC2{ get; set; }
        public int? QTY1 { get; set; } = 0;
        public int? QTY2 { get; set; } = 0;
        public int? TransactionTotalAmount { get; set; } = 0;
        public int? Type { get; set; }   
    }
}




