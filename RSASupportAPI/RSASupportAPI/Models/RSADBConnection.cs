namespace RSASupportAPI.Models
{
    public class RSADBConnection
    {
        public string DataSource { get; set; }
        public String Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    public class ConnectionStrings
    {
        public string RSAGroceryDBCon { get; set; }
    }
}
