using Amazon.S3.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace RSASupportAPI.Models
{
    public class AllShoppersGroups
    {
        public string GroupName { get; set; }
        public string GroupDetails { get; set; }
        public int GroupID { get; set; }
        public string CreatedOn { get; set; }
        public int TotalShoppers { get; set; }
        public string TopicARN { get; set; }    
    }

    public class ProductsModel
    {
        public string ProductName { get; set; }
        public string UPC { get; set; }
        public decimal AMOUNT { get; set; }
    }
    public class Club
    {
        public int ClubId { get; set; }
        public string Name { get; set; }    
        public string ClubDetails { get; set; }
        public string CreatedDate { get; set; }
    }
    public class GroupsModels
    {
        public int TOTALSHOPPERS { get; set; }
        public int TOTALTRANSACTIONS { get; set; }
        public decimal TOTALCOUPONVALUE { get; set; }
        public decimal TOTALINCOME { get; set; }
        public decimal AVGTRANSACTION { get; set; }
        public string GROUPSTATUS { get; set; }

        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
    }
    public class GroupAnalysysTimeLineResponse
    {
        public Club GroupDetails { get; set; }
        public GroupsModels GroupStartDetails { get; set; }
        public GroupsModels GroupSummeryDetails { get; set; }
        public GroupsModels Fromdays1 { get; set; }
        public GroupsModels Fromdays2 { get; set; }
        public GroupsModels Fromdays3 { get; set; }
        public GroupsModels Fromdays4 { get; set; }
        public GroupsModels Fromdays5 { get; set; }
        public GroupsModels Fromdays6 { get; set; }


    }

    public class DownloadShoppers
    {
        public string UserName { get; set; }
        public string BarcodeValue { get; set; }
        public string StoreName { get; set; }
    }
    public class TopShoppers
    {
     public long   LOYALTYID { get; set; } 
     public string FIRSTNAME { get; set; }
     public string LASTNAME { get; set; }
     public string USERNAME { get; set; }
    public long USERDETAILID { get; set; }
    public long STOREID { get; set; }
    public string STORENAME { get; set; }
    //public int VISITSCOUNT { get; set; }
        //public decimal TOTAL { get; set; }
        //public decimal AVGBASKET { get; set; }
        // public decimal TOTALBASKETAMOUNT { get; set; }  
      
        public long VISITSCOUNT { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal TOTAL { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal AVGBASKET { get; set; }

        [Range(0.00, double.MaxValue)]
        public decimal TOTALBASKETAMOUNT { get; set; }
    }

    public class ProductCategories
    {
       public int ProductCategoryId { get; set; }
       public string ProductCategoryName { get; set; }
       public int ClientDepartmentID { get; set;  }
       public int DefaultProductID { get; set; }
       public int MajorDepartmentID { get; set; } 
    }

    public class AdvancedSearchShopper
    {
  //public long loyaltyid { get; set; }
  public string loyaltyid { get; set; }
   public string UserName { get; set; }
  public long UserDetailId { get; set; }
  public decimal AVGTrAmount      { get; set; }
  public int BasketDataCount { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public int TotalRecords  { get; set; }
  public string PreferredStore {  get; set; } 

    }



}


     