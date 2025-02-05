namespace RSASupportAPI.Models
{
    public class FileModel
    {
    }
    public class UPCUpload()
    {
        public string UPC { get; set; }
    }
    public class MemberNumberUpload()
    {
        public string MemberNumber { get; set; }
    }
    public class FileJsonDataItem
    {
        public string UPCCodes { get; set; }
    }

    public class UploadResponse
    {
        public List<FileJsonDataItem> FileJsonData { get; set; }
        public string StringJsonFiledata { get; set; }
    }


}
