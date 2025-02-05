using System.Security.Permissions;
using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace RSASupportAPI.Models
{
    public class Coupon
    {
    }

    public class SearchCoupons
    {
        public int NewsID { get; set; }
        public int NewsCategoryID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string OtherDetails { get; set; }    
        public string ProductImage { get; set; }
        public string ImagePath { get; set; }
        //  public string ValidFromDate { get; set; }
        public string ValidFromDate { get; set; }
        public string ExpiresOn { get; set; }
        public bool SendNotification { get; set; }
        public int CustomerID { get; set; }
        public int CreateUserID { get; set; }
        public int UpdateUserID { get; set; }
        public string CustomerName { get; set; }
        // public string ShutOffDate { get; set; }
        public string CategoryName { get; set; }
        public string PUICode { get; set; }
        public Guid? CouponUniqueId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsDiscountPercentage { get; set; }
        public string NCRPromotionCode { get; set; }
        public string NCRPromotionCreatedDate { get; set; }
        public bool IsItStoreSpecific { get; set; }

        public int ManufacturerCouponId { get; set; }

        public int ProductQuantity { get; set; }
        public int UpSellProductId { get; set; }
        public int UpSellProductQuantity { get; set; }
        public bool IsFeatured { get; set; }
        //public string ProductCode { get; set; }
        public bool IsItTargetSpecific { get; set; }
        public bool IsRecurring { get; set; }
        public int RedeemCount { get; set; }
        public bool IsDealOftheWeek { get; set; }
        public string MFGShutOffDate { get; set; }
        public int ClipsCount { get; set; }
        public int ParentNewsId { get; set; }
        public int CouponLimit { get; set; }
        public bool IsPosIntegrationEnabled { get; set; }
        public int OfferTypeId { get; set; }


        //public string OtherDetails { get; set; }

        // public int? RecurringId { get; set; }


        // public string NCRPromotionID { get; set; }
        // public string RecurringEndDate { get; set; }

        // public bool ISMultipleUPCCoupon { get; set; }


    }

    public class NewsCategories
    {
        public int NewsCategoryId { get; set; }
        public string CategoryName { get; set; }

    }

    public class Specials
    {
        public int SSNewsId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ImagePath { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool SendNotification { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string NewsCategoryDescription { get; set; }
        public bool IsWeeklyCoupons { get; set; }
        public string ProductName { get; set; }
        public decimal Amoount { get; set; }
        public decimal DiscountAmoount { get; set; }
        public bool IsDiscountPercentage { get; set; }
        public int ProductId { get; set; }
        public string RedeemDate { get; set; }
        public bool IsRedeem { get; set; }
        public bool IsInCart { get; set; }
        public string PLUCode { get; set; }
        public string NCRPromotionCode { get; set; }
        public bool IsInNCRImpressions { get; set; }
        public int NewsCategoryId { get; set; }
        public bool IsFeatured { get; set; }
        public int ProductCategoryId { get; set; }
        public string SpecialPrice { get; set; }
        public string DepartmentName { get; set; }
        public int CouponLimit { get; set; }
        public string ExpiresOnDateString { get; set; }
        public string NoOfDaysLeftString { get; set; }
    }

    //public class SaveBasketModel
    //{
    //    public int NewsID { get; set; }
    //    public int NewsCategoryID { get; set; }
    //    public string Title { get; set; }
    //    public string Details { get; set; }
    //    public string ImagePath { get; set; }
    //    public DateTime ValidFromDate { get; set; }
    //    public DateTime ExpiresOn { get; set; }
    //    public bool SendNotification { get; set; }
    //    public int CustomerID { get; set; }
    //    public int CreateUserID { get; set; }
    //    public int UpdateUserID { get; set; }
    //    public string PUICode { get; set; }
    //    public int ProductId { get; set; }
    //    public decimal Amount { get; set; }
    //    public decimal DiscountAmount { get; set; }
    //    public bool IsDiscountPercentage { get; set; }
    //    public string NCRPromotionCode { get; set; }
    //    public bool IsItStoreSpecific { get; set; }
    //    public long ManufacturerCouponId { get; set; }
    //    public int ProductQuantity { get; set; }
    //    public int UpSellProductId { get; set; }
    //    public int UpSellProductQuantity { get; set; }
    //    public bool IsFeatured { get; set; }
    //    public bool DeleteFlag { get; set; }
    //    public bool IsItTargetSpecific { get; set; }
    //    public string OtherDetails { get; set; }
    //    public bool IsRecurring { get; set; }
    //    public DateTime MfgShutOffDate { get; set; }
    //    public bool IsDealOftheWeek { get; set; }
    //    public int? News_Id { get; set; } // Nullable to handle OUTPUT parameter
    //    public int DepartmentId { get; set; }
    //    public bool IsMajorDepartment { get; set; }
    //    public string StoreID { get; set; } // Comma-separated string of StoreIDs
    //    public int PageNumber { get; set; }
    //    public string PdfFileName { get; set; }
    //    public int ClubId { get; set; }
    //    public int UserDetailId { get; set; }
    //    public int ClubMemberId { get; set; }
    //    public int Id { get; set; }
    //    public string NEWS_ID { get; set; }
    //    public string StoreRouteId { get; set; }
    //    public int ClientStoreId { get; set; }
    //}



    //   using System.Text.Json.Serialization;

    public class SaveBasketModel
    {
        [JsonPropertyName("newsID")]
        public int NewsID { get; set; }

        [JsonPropertyName("newsCategoryId")]
        public int NewsCategoryID { get; set; } 

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public string Details { get; set; } = string.Empty;

        [JsonPropertyName("imagePath")]
        public string ImagePath { get; set; } = string.Empty;

        [JsonPropertyName("validFromDate")]
        public DateTime ValidFromDate { get; set; }

        [JsonPropertyName("expiresOn")]
        public DateTime ExpiresOn { get; set; }

        [JsonPropertyName("sendNotification")]
        public bool SendNotification { get; set; }

        [JsonPropertyName("customerId")]
        public int CustomerID { get; set; }

        [JsonPropertyName("createUserId")]
        public int CreateUserID { get; set; }

        [JsonPropertyName("updateUserId")]
        public int UpdateUserID { get; set; }

        [JsonPropertyName("puiCode")]
        public string PUICode { get; set; } = string.Empty;

        [JsonPropertyName("productId")]
        public int? ProductId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("discountAmount")]
        public decimal DiscountAmount { get; set; }

        [JsonPropertyName("isDiscountPercentage")]
        public bool IsDiscountPercentage { get; set; }

        [JsonPropertyName("ncrPromotionCode")]
        public string NCRPromotionCode { get; set; } = string.Empty;

        [JsonPropertyName("isItStoreSpecific")]
        public bool IsItStoreSpecific { get; set; }

        [JsonPropertyName("manufacturerCouponId")]
        public long? ManufacturerCouponId { get; set; }

        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonPropertyName("upSellProductId")]
        public int UpSellProductId { get; set; }

        [JsonPropertyName("upSellProductQuantity")]
        public int UpSellProductQuantity { get; set; }

        [JsonPropertyName("isFeatured")]
        public bool IsFeatured { get; set; }

        [JsonPropertyName("deleteFlag")]
        public bool DeleteFlag { get; set; }

        [JsonPropertyName("isItTargetSpecific")]
        public bool IsItTargetSpecific { get; set; }

        [JsonPropertyName("otherDetails")]
        public string OtherDetails { get; set; } = string.Empty;

        [JsonPropertyName("isRecurring")]
        public bool IsRecurring { get; set; }

        [JsonPropertyName("mfgShutOffDate")]
        public DateTime? MfgShutOffDate { get; set; }

        [JsonPropertyName("isDealOfTheWeek")]
        public bool IsDealOftheWeek { get; set; }

        [JsonPropertyName("departmentId")]
        public string DepartmentId { get; set; }

        [JsonPropertyName("isMajorDepartment")]
        public bool IsMajorDepartment { get; set; }

        [JsonPropertyName("storeId")]
        public string StoreID { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pdfFileName")]
        public string PdfFileName { get; set; } = string.Empty;

        [JsonPropertyName("clubId")]
        public int ClubId { get; set; }

        [JsonPropertyName("userDetailId")]
        public int UserDetailId { get; set; }

        [JsonPropertyName("clubMemberId")]
        public int ClubMemberId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("storeRouteId")]
        public string StoreRouteId { get; set; } = string.Empty;

        [JsonPropertyName("clientStoreId")]
        public int ClientStoreId { get; set; }

        [JsonPropertyName("news_Id")]
        public string News_ID { get; set; } = string.Empty;

        [JsonPropertyName("recurringStartDate")]
        public DateTime RecurringStartDate { get; set; }

        [JsonPropertyName("recurringEndDate")]
        public DateTime RecurringEndDate { get; set; }

        [JsonPropertyName("recurringTypeId")]
        public int RecurringTypeId { get; set; }

        [JsonPropertyName("clubIds")]
        public string ClubIds { get; set; }
        [JsonPropertyName("groupNames")]
        public string GroupNames { get; set; } = string.Empty;
        [JsonPropertyName("clientStoreIds")]
        public string ClientStoreIds { get; set; }

    }

    public class SaveUPCPromotionsModel
    {
        public int NewsID { get; set; }
        public int NewsCategoryID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ImagePath { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool SendNotification { get; set; }
        public int CustomerID { get; set; }
        public int CreateUserID { get; set; }
        public int UpdateUserID { get; set; }
        public string PUICode { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsDiscountPercentage { get; set; }
        public string NCRPromotionCode { get; set; }
        public bool IsItStoreSpecific { get; set; }
        public long ManufacturerCouponId { get; set; }
        public int ProductQuantity { get; set; }
        public int UpSellProductId { get; set; }
        public int UpSellProductQuantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool DeleteFlag { get; set; }
        public bool IsItTargetSpecific { get; set; }
        public string OtherDetails { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime MfgShutOffDate { get; set; }
        public bool IsDealOftheWeek { get; set; }
        public string News_ID { get; set; }
        public string DepartmentId { get; set; }
        public bool IsMajorDepartment { get; set; }
        public string StoreID { get; set; }
        public int PageNumber { get; set; }
        public string PdfFileName { get; set; }
        public int ClubId { get; set; }
        public int UserDetailId { get; set; }
        public int ClubMemberId { get; set; }
        public int Id { get; set; }
        public string StoreRouteId { get; set; }
        public string ClientStoreId { get; set; }
        public DateTime RecurringStartDate { get; set; }
        public DateTime RecurringEndDate { get; set; }
        public int RecurringTypeId { get; set; }
        public string ClubIds { get; set; }
        public string GroupNames { get; set; }
        public string ClientStoreIds { get; set; }
        public string UPC { get; set; }
        public string ProductName { get; set; }
        public int Product_ID { get; set; }
    }

    public class CouponsModel
    {
        public int NewsID { get; set; }
        public int NewsCategoryID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ProductImage { get; set; }
        public string ImagePath { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool SendNotification { get; set; }
        public int CustomerID { get; set; }
        public int CreateUserID { get; set; }
        public int UpdateUserID { get; set; }
        public string CustomerName { get; set; }
        public string CategoryName { get; set; }
        public string PUICode { get; set; }
        public string CouponUniqueId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsDiscountPercentage { get; set; }
        public string NCRPromotionCode { get; set; }
        public DateTime NCRPromotionCreatedDate { get; set; }
        public bool IsFeatured { get; set; }
        public int ProductCategoryId { get; set; }
        public DateTime? RedeemOn { get; set; } // Nullable since it is fetched conditionally
        public int ClipsCount { get; set; }
        public decimal? SpecialPrice { get; set; } // Nullable since it is fetched conditionally
        public string DepartmentName { get; set; }
        public bool IsItTargetSpecific { get; set; }
        public int RedeemCount { get; set; }
        public int IsExclude { get; set; }
    }


}
