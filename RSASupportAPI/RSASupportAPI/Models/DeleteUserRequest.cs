using System.Runtime.Serialization;

namespace RSASupportAPI.Models
{
    public class DeleteUserRequest
    {
        public int UserDetailId { get; set; }
       
        public string MemberNumber { get; set; }
    }
}
