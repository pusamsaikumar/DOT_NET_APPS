using Amazon.S3.Model;
using System;

namespace RSASupportAPI.Models
{
    public class LMRewards
    {
        //public DateTime ValidFrom { get; set; }
        //public DateTime ExpiresOn { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string RewardTitle { get; set; }
        public bool IsPointsBased { get; set; }
        public string ValidFrom { get; set; }
        public string ExpiresOn { get; set; }
      
       
    }

    public class LMRewardResponse
    {
        public List<LMRewards> Rewards { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class RewardTypes
    {
        public int RewardType { get; set; }
        public string Description { get; set; }
    }

    public class MatchRewards
    {
       public int LMREWARDID { get; set; } 
       public int RewardTypeID {  get; set; } 
       public int BuyQtyAmount { get; set; }
        public int RewardQtyAmount { get; set; }
        public string ValidFrom { get;set; }
        public string ExpiresOn { get; set; }
        public decimal RewardQtyAmountMoney { get; set; }
        public decimal PointsPerEach { get; set; }
    }
    
    public class MyLMRewards
    {
        public string ImageURL { get; set; }
        public int BuyQtyAmount { get; set;} 
        public int RewardQtyAmount { get;set;}
        public decimal RewardQtyAmountMoney { get; set;}
        public decimal PointsPerEach { get; set; } 
        public string StringFormattedPoints { get; set; }   
    }
    
    public class FindMemberNumber
    {
       // public DateTime ValidFrom { get; set; }
       public string ValidFrom { get; set; }

        //public DateTime ExpiresOn { get; set; }
        public string ExpiresOn { get; set; }
        public string Title { get; set; }
        public int RewardTypeID { get; set; }
        public int BuyQtyAmount { get; set; }
        public int RewardQtyAmount { get; set; }
        public string RewardTitle { get; set; } 
        public string AdditionalDetails { get; set; }  
        
        public string POSDetails { get; set; }  
        public string ImageURL { get; set;} 
        public int CreatedUserID { get; set; }  
       // public DateTime CreatedDateTime { get; set; }
        public string CreatedDateTime { get; set; }
        public int RewardGroupID { get; set; }
        public int CouponID { get; set; }
        public int RewardStatus { get; set; }
        public int RewardDepartmentID { get; set; }
        //public DateTime RewardMustBeUsedByDate { get; set; }
        public string RewardMustBeUsedByDate { get; set; }
        
        public bool IsTargetSpecific { get; set; }
        public bool IsDiscountPercentage { get; set; }
        public int RewardCouponMinQty { get; set; }
        public int RewardCouponTypeID { get; set; }
        public decimal RewardQtyAmountMoney { get; set; }
        public bool IsDepartmentSpecific { get; set; }
        public bool IsStoreSpecific { get; set; }
        public decimal PointsPerEach { get; set; }
        public Boolean IsPointsBased { get; set; }  
        public int TierValue { get; set; }  
        public int NumberOfVisits  { get; set; }
    }

}
