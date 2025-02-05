namespace RSASupportAPI.Models
{
    public class PreDifinedShopperGroups
    {
    }
    public class PreDefinedShopperGroupsByLastShopped
    {
        public int NoOfDaysSinceShopped { get; set; }
        public string GroupName { get; set; }
    }

   public class PreDefinedShopperGroupByZipcodes
    {
      public string   ZipcodeList {  get; set; }
      public int AllUsers { get; set; }
      public DateTime? SinceRegistered { get; set; } = default(DateTime?);


 
    }
    public class PreDefinedShopperGroupsByUPCLists
    {
        public string GroupName { get; set; }
        public string UPCList { get; set; } 
        public int NoOfTimesPurchased { get; set; }
        public int NoOfDaysSinceShopped { get; set; }   

    }
    public class ResponsePredefinedShopperGroups
    {
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public string Status { get; set; }
    }
    }
