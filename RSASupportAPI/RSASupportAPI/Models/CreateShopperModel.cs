namespace RSASupportAPI.Models
{
    public class CreateShopperModel
    {
       
        public string UserName { get; set; }
      
        
        public string Password { get; set; }    
        public string ConfirmPassword { get; set; } 
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
      

       
       
        public int? UserTypeId { get; set; }
   
        public string ZipCode { get; set; }
        public string Mobile { get; set; }

        public string RoleName { get; set; }
        public bool IsActive { get; set; }

    }
}
