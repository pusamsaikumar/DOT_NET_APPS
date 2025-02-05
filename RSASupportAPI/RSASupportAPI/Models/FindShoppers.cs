using Amazon.S3.Model;

namespace RSASupportAPI.Models
{
    public class FindShoppers
    {
        public int UserDetailId { get; set; }   
        public string?  Email { get; set; }
        public string? BarCodeValue { get; set; }
        public string? Mobile { get; set; }
        public string? ZipCode { get; set; }
        public int? ClientStoreId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? SignUpDate { get; set; }
    }
    public class FindShopperPagination
    {
       public List<FindShoppers> FindShopperList { get; set; }
        public List<FindShoppers> SearchFindShopper { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
    public class FindShopper
    {
        public string? Email { get; set; }
        public string? BarCodeValue { get; set; }
        public string? Mobile { get; set; }
        public string? ZipCode { get; set; }
        public int? ClientStoreId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? SignUpDate { get; set; }
    }

    public class FindShopperByUPCsList
    {
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string USERNAME { get; set; }
        public int USERDETAILID { get; set; }  
        public decimal TRANSACTIONAMOUNT { get; set; }  
        public string PREFERREDSTORE { get; set; }   
        
    }

    public class  ProductsDetails
    {
         public int   ProductId { get; set; }
        public string ProductCode { get; set; }   
        public int ProductCategoryId { get; set; }
        public string   ProductName { get; set; }
        public string   Description { get; set; }
        public decimal? SalePrice { get; set; }    
        public string ProductCategoryName { get; set; }
        public string  ProductImage { get; set; } 
   }
    









}

