namespace RSASupportAPI.Models
{
    public class UserBasketTransactions
    {
        public int UserId { get; set; }
        public int BasketDataID { get; set; }
        public string LoyaltyId { get; set; }
        public int? Storeid { get; set; }
        //public DateTime TransactionDate { get; set; }
        public string TransactionDate { get; set; }
        public decimal? TotalBasketAmount { get; set; }
        public string StoreName { get; set; }   
        public long? RowNumber {  get; set; }  
        public string DropDownName { get; set; }
        public string DropDownKey { get; set; }

    }
}
