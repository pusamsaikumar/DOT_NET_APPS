namespace RSASupportAPI.Models
{
    public class AspnetMembershipCreateUser
    {
        public string ApplicationName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsAproved { get; set; }
        public DateTime CurrentTimeUtc { get; set; }
        public DateTime CreateDate { get; set; }
        public int UniqueEmail { get; set; }
        public int PasswordFormat { get; set; }
        public Guid UserId { get; set; }
    }
    public class Get_aspnet_userId_result
    {
        public Guid UserId { get; set; }
    }

    public class BarCodeResult
    {
        public string BarCodeImage { get; set; } = string.Empty;
        public string BarCodeValue { get; set; } = string.Empty;

    }



    public class QRCodeResult
    {
        public string QRCodeImage { get; set; }
        public string QRCodeValue { get; set; }
    }

    
    public class UserDetailstoSave
    {
        public int UserDetailId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        //  public  Nullable<System.Guid> UserGUID { get; set; }    
        public Guid? CustomerId { get; set; }
        public int UserId { get; set; }
        public string CustomerCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DeviceType { get; set; }
        public int DeviceId { get; set; }
        public int UserStatusId { get; set; }
        public bool IsDeleted { get; set; }
        public string BarCodeImage { get; set; }
        public string BarCodeValue { get; set; }
        public string QRCodeImage { get; set; }
        public string QRCodeValue { get; set; }
        public int? CompanyCustomerFK { get; set; }
        public int? UserTypeId { get; set; }
        public int? CompanyCustomerId { get; set; }
        public string ZipCode { get; set; }
        public int ClientId { get; set; }
        public int ClientStoreId { get; set; }
        public string QToken { get; set; }

        public string ExistingMemberNumber { get; set; }
    }

    
    public class ResponseShopper
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string Status { get; set; }
    }
    public class UserTypes
    {
        public int Id { get; set; }
        public string UserType { get; set; }
    }
    public class GetRoles
    {
        //public Guid RoleId { get; set; }
        public string RoleId    { get; set; }
        public string RoleName { get; set; }
    }
}
