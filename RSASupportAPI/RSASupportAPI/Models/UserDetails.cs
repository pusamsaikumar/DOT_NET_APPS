using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace RSASupportAPI.Models
{
    public class UserDetails
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MemberNumber { get; set; }

    }
    public class Client
    {
        //public List<string> clientNames { get; set; }
        public object clientNames { get; set; } 
    }
    public class ClinetResponse
    {
        //public List<Client> Clients { get; set; }

        //  public List<string> Clients { get; set; }   
        public Client Clients { get; set; }
        public string ErroCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ClientDataResponse
    {
        public UserDetails ClientInfo { get; set; }
        public string ErroCode { get; set; }
        public string ErrorMessage { get; set; }
    }
   
}
