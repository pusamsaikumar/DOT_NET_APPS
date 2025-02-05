using System.Reflection.Emit;

namespace RSASupportAPI.Models
{
    public class CreateShopperGroupModel
    {
       public string SIGNFROMDATE { get; set; }
       public  string SIGNUPTODATE { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string USERNAME { get; set; }
        public string  ZIPCODE { get; set; }
        public int STOREID { get; set; }    
        public string MEMBERNUMBER { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

    }

}
