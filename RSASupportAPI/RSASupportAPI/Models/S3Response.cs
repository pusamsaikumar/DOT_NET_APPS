namespace RSASupportAPI.Models
{
    public class S3Response
    {
    }
    public class BucketResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public string Status { get; set; }
    }
    public class DeleteResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public string Status { get; set; }
    }
}
