using Microsoft.AspNetCore.Mvc;
using RSASupportAPI.Models;
using System.Threading.Tasks;

namespace RSASupportAPI.RSASupportDAL
{
    public interface IClientRepo
    {

        Task<Client> GetClients();
        Task<List<dynamic>> GetClientData(string clientName, string selectQuery);
        //Task<List<dynamic>> GetLMReward(string clientName, string selectQuery);
        Task<List<LMRewards>> GetLMRewards(string clientName);
        Task<List<FindShoppers>> GetFindShopperDetails(string clientName,
            string? email,
            string? memberNumber,
            string? mobileNumber,
            string? zipcode,
            int? clientStoreId,
            string? firstName,
            string? lastName,
            DateTime? signUpFromDate,
            DateTime? signUpToDate);

        Task<List<ClientStores>> GetClientStores(string clientName);
        Task<List<RewardTypes>> GetRewardTypes(string clientName);
        Task<FindShopper> GetFindShopperDetailsById(string clientName, int userDetailId);
        Task<FindShopper> UpdateUserDetails(string clientName, int userDetailId, FindShopper findShopper);
        Task<List<UserClipsAndRedemptionDates>> GetUserClipsAndRedemptionDates(int userId);
        Task<List<UserRewardCoupons>> GetUserRewardCoupons(int userDetailId);
        Task<List<PROC_CUSTOM_VIEW_USER_HISTORY>> GetUserHistory(int userDetailId, string clientName);
        Task<List<UserBasketTransactions>> GetUserBasketTransactions(int userId, string clientName);
        Task<List<GetUserRecentPurchasedProducts>> GetUserRecentPurchasedProducts(int userId, int basketDataId, string clientName);
        Task<List<UserPurchasedCoupons>> UserPurchasedCoupons(int userId, int basketDataId, string clientName);
        Task<List<MatchRewards>> GetMatchedRewardsForUser(string memberNumber, string clientName);
        Task<List<MatchRewards>> GetMatchedRewardsForUserWithMemberNumber(string memberNumber, string clientName);
        Task<List<MyLMRewards>> GetMyLMRewardsForUser(int lmRewardId, string clientName);
        Task<List<FindMemberNumber>> GetFindMemberDetails(int lmRewardId, int rewardTypeId, string clientName);
        Task<UserPoints> SaveUserPoints(string ClientName, string MemberNumber, string? UPC1, int? Qty1, string? UPC2, int? Qty2, int? TransactionAmount, int? RewardTypeId);
        Task<DeleteUserRequest> DeleteUser(string clientName, DeleteUserRequest deleteUserRequest);
        Task<FindShopperPagination> GetPaginationData(
             //List<FindShoppers> findShopperData,
             string clientName, int? page, int? limit, string? email, string? barcodeValue,
             string? mobile, string? zipCode, int? clientStoreId, string? firstName,
             string? lastName, DateTime? signUpFromDate, DateTime? signUpToDate, string? sortColumn, string? sortDirection);
        Task<FindShopperPagination> GetFilteredUserDetails(
            string clientName, int? page, int? limit, string? sortColumn, string? sortDirection, string? searchTerm);
        //Task<List<CreateShopperGroupModel>> CreateShopperGroups(string clientName, CreateShopperGroupModel model);
        Task<List<CreateShopperGroupModel>> CreateShopperGroups(string clientName, string? SIGNFROMDATE, string? SIGNUPTODATE, string? FIRSTNAME, string? LASTNAME,
            string? USERNAME, string? ZIPCODE, int? STOREID, string? MEMBERNUMBER, string? GroupName, string? Description, int UserDetailId);
        Task<List<UserGroups>> GetUserGroups(string clientName, int userId);
        Task<List<UserGroups>> GetAvailableUserGroups(string clientName, int userId);
        Task<int> AddGroups(string clientName, int userId, int clubId);
        Task<int> DeleteUserGroups(string clientName, int userId, int clubId);
        Task<int> CreateMembershipUser(string clientName, AspnetMembershipCreateUser aspnetMembershipCreateUser);
        Task<Get_aspnet_userId_result> GetUserIdbyUserName(string clientName, string userName);
        Task<BarCodeResult> GetBarcode(int customerId);
        Task<int> CreateRole(string clientName, string applicationName, string roleName);
        Task<int> ValidateUser(string clientName, string userName);
        Task<ResponseShopper> CreateShopper(string clientName, CreateShopperModel model);
        // Task<int> ValidateExistingRole(string clientName,string applicationName,string roleName);
        Task<int> ValidateExistingRole(string clientName, string roleName);
        Task<int> SaveShopperDetails(string clientName, UserDetailstoSave model);
        Task<List<UserTypes>> GetUserTypes(string clientName);
        Task<List<GetRoles>> GetAllRoles(string clientName);

        Task<List<AllShoppersGroups>> GetAllShoppersGroups(string clientName, int groupId, int userId);
        Task<List<ProductsModel>> GetAllTimeProducts(string clientName, int GroupId);
        Task<List<ProductsModel>> GetTopProducts(string clientName, int GroupId, int NoOfDays);
        Task<GroupAnalysysTimeLineResponse> GetGroupAnalysisTimeLine(string clinetName, int groupId, int userId);
        Task<List<DownloadShoppers>> DownloadUserShopperReport(string clientName, int groupId);
        Task<int> PreDefinedShopperGroupsByLastShoppedDate(string clientName, PreDefinedShopperGroupsByLastShopped model);
        Task<int> PreDefinedShopperGroupByZipcodesList (string clientName, PreDefinedShopperGroupByZipcodes model);
        Task<int> PreDefinedShopperGroupsByUPCList(string clientName, PreDefinedShopperGroupsByUPCLists model);
        Task<List<ProductCategories>> GetProductCategories(string clientName);
        Task<List<TopShoppers>> GetTopShoppers(string clientName, int noOfRecords, int storeId, string orderByDirection, DateTime? fromDate, DateTime? toDate, int? departmentId);
        Task<List<AdvancedSearchShopper>> GetAdvancedSearchShoppers(string clientName, string? memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string? clubId, int? minBasketCount, int? maxBasketCount, string? storeId, int? minReedemCount, int? maxRedeemCount, string? departmentId, bool? isCreatedGroup);
        
        Task<List<AdvancedSearchShopper>> GetShoppersAdvancedSearch(string memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string clubId, int? minBasketCount, int? maxBasketCount, string storeId, int? minReedemCount, int? maxRedeemCount, string departmentId, bool? isCreatedGroup);
        Task<List<FindShopperByUPCsList>> GetFindShopperByUPCs(string clientName, string? UPC, int NoOfCoupons, DateTime? @FromDate, DateTime? ToDate);
        Task<List<ProductsDetails>> GetProductDetailsupc(string clientName, string? productCode, string? productName, int productCategoryId, bool? isMajorDepartment);
        Task<List<AllShoppersGroups>> GetSearchAndCountDetails(string clientName, int minDays, int maxDays, string groupName);
        Task<List<AllShoppersGroups>> GetSearchAndCreateGroup(string clientName, int minDays, int maxDays, string groupName);
        Task<int> AddGroupWithSearchDetails(string clientName, int minDays, int maxDays, string groupName);
        Task<int> UploadShoppersToGroupsWithFile(string clientName, string groupName, string memberNumbers);
        Task<List<SearchCoupons>> FindCoupons(string clientName, int newsCategoryId, DateTime? valid, DateTime? expires);
        Task<int> CreateBasketCoupon(string clientName,SaveBasketModel model);
        Task<List<NewsCategories>> GetNewsCategories(string clientName);
        Task<int> SaveUPCPromotion(string clientName, SaveUPCPromotionsModel model);

    }

}
