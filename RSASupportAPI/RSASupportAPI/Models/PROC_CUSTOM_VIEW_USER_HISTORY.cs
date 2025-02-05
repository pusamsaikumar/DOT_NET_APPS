namespace RSASupportAPI.Models
{
    public class PROC_CUSTOM_VIEW_USER_HISTORY
    {
        public int UserDetailId { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Mobile {  get; set; }
        public string ZipCode { get; set; }
        public string BarcodeValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? UserRank { get; set; }
        public string UserClubNames { get; set; }   
        public int TotalbasketCount { get; set; }   
        public decimal? AvgBasketsAmount { get; set; }
        public int Clips { get; set; }
        public int Redemptions { get; set; }    

    }
}



