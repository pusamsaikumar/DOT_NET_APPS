using RSASupportAPI.Models;
using RSASupportAPI.RSASupportDAL;
using System.Data.SqlClient;

namespace RSASupportAPI.RSASupportBLL
{
    public class ClientService : IClientService
    {
        private readonly IClientRepo _clientRepo;

        public ClientService(IClientRepo clientRepo)
        {
            _clientRepo = clientRepo;
        }

        // public async Task<Dictionary<string, object>> GetClientData(string clientName, string selectQuery)
        //  public async Task<dynamic> GetClientData(string clientName, string selectQuery)
        public async Task<List<dynamic>> GetClientData(string clientName, string selectQuery)
        {
            var result = await _clientRepo.GetClientData(clientName, selectQuery);
            return result;
        }

        //public async Task<UserDetails> GetClientData(string clientName, string selectQuery)
        //{
        //   var result = await _clientRepo.GetClientData(clientName, selectQuery);
        //   return result;
        //}

        public async Task<Client> GetClients()
        {
            var result = await _clientRepo.GetClients();
            return result;
        }

        public async Task<List<FindShoppers>> GetFindShopperDetails(string clientName, string? email, string? memberNumber, string? mobileNumber, string? zipcode, int? clientStoreId, string? firstName, string? lastName, DateTime? signUpFromDate,
            DateTime? signUpToDate)
        {
            var result = await _clientRepo.GetFindShopperDetails(clientName, email, memberNumber, mobileNumber, zipcode, clientStoreId, firstName, lastName, signUpFromDate, signUpToDate);
            return result;
        }

        public async Task<List<LMRewards>> GetLMReward(string clientName)
        {
            var result = await _clientRepo.GetLMRewards(clientName);
            return result;
        }
        public async Task<List<ClientStores>> GetClientStores(string clientName)
        {
            var result = await _clientRepo.GetClientStores(clientName);
            return result;
        }
        public async Task<List<RewardTypes>> GetRewardTypes(string clientName)
        {
            var result = await _clientRepo.GetRewardTypes(clientName);
            return result;
        }
        public async Task<FindShopper> GetFindShopperDetailsById(string clientName, int userDetailId)
        {
            var result = await _clientRepo.GetFindShopperDetailsById(clientName, userDetailId);
            return result;
        }

        public async Task<FindShopper> UpdateUserDetails(string clientName, int userDetailId, FindShopper findShopper)
        {
            var result = await _clientRepo.UpdateUserDetails(clientName, userDetailId, findShopper);
            return result;
        }
        public async Task<List<UserClipsAndRedemptionDates>> GetUserClipsAndRedemptionDates(int userId)
        {
            var result = await _clientRepo.GetUserClipsAndRedemptionDates(userId);
            return result;
        }
        public async Task<List<UserRewardCoupons>> GetUserRewardCoupons(int userDetailId)
        {
            var result = await _clientRepo.GetUserRewardCoupons(userDetailId);
            return result;
        }
        public async Task<List<PROC_CUSTOM_VIEW_USER_HISTORY>> GetUserHistory(int userDetailId, string clientName)
        {
            var result = await _clientRepo.GetUserHistory(userDetailId, clientName);
            return result;
        }
        public async Task<List<UserBasketTransactions>> GetUserBasketTransactions(int userId, string clientName)
        {
            var result = await _clientRepo.GetUserBasketTransactions(userId, clientName);
            return result;
        }

        public async Task<List<GetUserRecentPurchasedProducts>> GetUserRecentPurchasedProducts(int userId, int basketDataId, string clientName)
        {
            var result = await _clientRepo.GetUserRecentPurchasedProducts(userId, basketDataId, clientName);
            return result;
        }
        public async Task<List<UserPurchasedCoupons>> UserPurchasedCoupons(int userId, int basketDataId, string clientName)
        {
            var result = await _clientRepo.UserPurchasedCoupons(userId, basketDataId, clientName);
            return result;
        }

        public async Task<List<MatchRewards>> GetMatchedRewardsForUser(string memberNumber, string clientName)
        {
            var result = await _clientRepo.GetMatchedRewardsForUser(memberNumber, clientName);
            return result;
        }

        public async Task<List<MyLMRewards>> GetMyLMRewardsForUser(int lmRewardId, string clientName)
        {
            var result = await _clientRepo.GetMyLMRewardsForUser(lmRewardId, clientName);
            return result;
        }
        public async Task<List<MatchRewards>> GetMatchedRewardsForUserWithMemberNumber(string memberNumber, string clientName)
        {
            var result = await _clientRepo.GetMatchedRewardsForUserWithMemberNumber(memberNumber, clientName);
            return result;
        }
        public async Task<List<FindMemberNumber>> GetFindMemberDetails(int lmRewardId, int rewardTypeId, string clientName)
        {
            var result = await _clientRepo.GetFindMemberDetails(lmRewardId, rewardTypeId, clientName);
            return result;
        }

        public async Task<UserPoints> SaveUserPoints(string ClientName, string MemberNumber, string? UPC1, int? Qty1, string? UPC2, int? Qty2, int? TransactionAmount, int? RewardTypeId)
        {
            var result = await _clientRepo.SaveUserPoints(ClientName, MemberNumber, UPC1, Qty1, UPC2, Qty2, TransactionAmount, RewardTypeId);
            return result;
        }
        public async Task<DeleteUserRequest> DeleteUser(string clientName, DeleteUserRequest deleteUserRequest)
        {
            var result = await _clientRepo.DeleteUser(clientName, deleteUserRequest);
            return result;
        }

        public async Task<FindShopperPagination> GetPaginationData(
            //List<FindShoppers> findShopperData,
            string clientName, int? page, int? limit, string? email, string? barcodeValue,
            string? mobile, string? zipCode, int? clientStoreId, string? firstName,
            string? lastName, DateTime? signUpFromDate, DateTime? signUpToDate, string? sortColumn, string? sortDirection)
        {
            var result = await _clientRepo.GetPaginationData(clientName, page, limit, email, barcodeValue, mobile, zipCode, clientStoreId, firstName, lastName, signUpFromDate, signUpToDate, sortColumn, sortDirection);
            return result;
        }

        public async Task<FindShopperPagination> GetFilteredUserDetails(string clientName, int? page, int? limit, string? sortColumn, string? sortDirection, string? searchTerm)
        {
            var result = await _clientRepo.GetFilteredUserDetails(clientName, page, limit, sortColumn, sortDirection, searchTerm);
            return result;
        }
        // public async Task<List<CreateShopperGroupModel>> CreateShopperGroups(string clientName, CreateShopperGroupModel model)
        public async Task<List<CreateShopperGroupModel>> CreateShopperGroups(string clientName, string? SIGNFROMDATE, string? SIGNUPTODATE, string? FIRSTNAME, string? LASTNAME,
            string? USERNAME, string? ZIPCODE, int? STOREID, string? MEMBERNUMBER, string? GroupName, string? Description, int UserDetailId)
        {
            var result = await _clientRepo.CreateShopperGroups(clientName, SIGNFROMDATE, SIGNUPTODATE, FIRSTNAME, LASTNAME, USERNAME, ZIPCODE, STOREID, MEMBERNUMBER, GroupName, Description, UserDetailId);
            return result;
        }
        public async Task<List<UserGroups>> GetUserGroups(string clientName, int userId)
        {
            var result = await _clientRepo.GetUserGroups(clientName, userId);
            return result;
        }

        public async Task<List<UserGroups>> GetAvailableUserGroups(string clientName, int userId)
        {
            var result = await _clientRepo.GetAvailableUserGroups(clientName, userId);
            return result;
        }

        public async Task<int> AddGroups(string clientName, int userId, int clubId)
        {
            var result = await _clientRepo.AddGroups(clientName, userId, clubId);
            return result;
        }
      public async  Task<int> DeleteUserGroups(string clientName, int userId, int clubId)
        {
            var result = await _clientRepo.DeleteUserGroups(clientName, userId, clubId);
            return  result;
        }
        public async Task<int> CreateMembershipUser(string clientName, AspnetMembershipCreateUser aspnetMembershipCreateUser)
        {
            var result = await _clientRepo.CreateMembershipUser(clientName, aspnetMembershipCreateUser);    
            return result;
        }

        public async Task<ResponseShopper> CreateShopper(string clientName, CreateShopperModel model)
        {
            var result = await _clientRepo.CreateShopper(clientName, model);
            return result;
        }
        public async Task<List<UserTypes>> GetUserTypes(string clientName)
        {
            var result = await _clientRepo.GetUserTypes(clientName);
            return result;
        }
        public async Task<List<GetRoles>> GetAllRoles(string clientName)
        {
             var result = await _clientRepo.GetAllRoles(clientName);
            return result;
        }
        public async Task<List<AllShoppersGroups>> GetAllShoppersGroups(string clientName, int groupId, int userId)
        {
            var result = await _clientRepo.GetAllShoppersGroups(clientName,groupId, userId);
            return result;
        }

        public async Task<List<ProductsModel>> GetAllTimeProducts(string clientName, int GroupId)
        {
            var result = await _clientRepo.GetAllTimeProducts(clientName,GroupId);  
            return result;
        }

        public async Task<List<ProductsModel>> GetTopProducts(string clientName, int GroupId, int NoOfDays)
        {
            var result = await _clientRepo.GetTopProducts(clientName,GroupId, NoOfDays);
            return result;
        }
        public async Task<GroupAnalysysTimeLineResponse> GetGroupAnalysisTimeLine(string clinetName, int groupId, int userId)
        {
            var result = await _clientRepo.GetGroupAnalysisTimeLine(clinetName, groupId, userId);
            return result;
        }
        public async Task<List<DownloadShoppers>> DownloadUserShopperReport(string clientName, int groupId)
        {
            var result = await _clientRepo.DownloadUserShopperReport(clientName, groupId);  
            return result;
        }
        public async Task<int> PreDefinedShopperGroupsByLastShoppedDate(string clientName, PreDefinedShopperGroupsByLastShopped model)
        {
            var result = await _clientRepo.PreDefinedShopperGroupsByLastShoppedDate(clientName, model); 
            return result;
        }
        public async Task<int> PreDefinedShopperGroupByZipcodesList(string clientName, PreDefinedShopperGroupByZipcodes model)
        {
            var result = await _clientRepo.PreDefinedShopperGroupByZipcodesList(clientName, model);
            return result;
        }
        public async Task<int> PreDefinedShopperGroupsByUPCList(string clientName, PreDefinedShopperGroupsByUPCLists model)
        {
            var result = await _clientRepo.PreDefinedShopperGroupsByUPCList(clientName, model);
            return result;
        }
        public async Task<List<ProductCategories>> GetProductCategories(string clientName)
        {
            var result = await _clientRepo.GetProductCategories(clientName);
            return result;
        }
        public async Task<List<TopShoppers>> GetTopShoppers(string clientName, int noOfRecords, int storeId, string orderByDirection, DateTime? fromDate, DateTime? toDate, int? departmentId)
        {
            var result = await _clientRepo.GetTopShoppers(clientName,noOfRecords, storeId, orderByDirection,fromDate,toDate,departmentId);
            return result;
        }

        public async Task<List<AdvancedSearchShopper>> GetAdvancedSearchShoppers(string clientName, string? memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string? clubId, int? minBasketCount, int? maxBasketCount, string? storeId, int? minReedemCount, int? maxRedeemCount, string? departmentId, bool? isCreatedGroup)
        {
          var result = await _clientRepo.GetAdvancedSearchShoppers(clientName, memberNumber, transactionFrom, transactionTo, minSpend, maxSpend, clubId, minBasketCount, maxBasketCount, storeId, minReedemCount, maxRedeemCount, departmentId, isCreatedGroup);
            return result;
        }

        public async Task<List<AdvancedSearchShopper>> GetShoppersAdvancedSearch(string memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string clubId, int? minBasketCount, int? maxBasketCount, string storeId, int? minReedemCount, int? maxRedeemCount, string departmentId, bool? isCreatedGroup)
        {
            var result = await _clientRepo.GetShoppersAdvancedSearch(memberNumber, transactionFrom, transactionTo, minSpend,maxSpend, clubId,minBasketCount, maxBasketCount,storeId, minReedemCount,maxRedeemCount, departmentId,isCreatedGroup);
            return result;
        }

        public async Task<List<FindShopperByUPCsList>> GetFindShopperByUPCs(string clientName, string? UPC, int NoOfCoupons, DateTime? FromDate, DateTime? ToDate)
        {
            var result = await _clientRepo.GetFindShopperByUPCs(clientName, UPC, NoOfCoupons,FromDate,ToDate); 
            return result;
        }

        public async Task<List<ProductsDetails>> GetProductDetailsupc(string clientName, string? productCode, string? productName, int productCategoryId, bool? isMajorDepartment)
        {
            var result = await _clientRepo.GetProductDetailsupc(clientName, productCode, productName, productCategoryId, isMajorDepartment);
            return result;
        }
        public async Task<List<AllShoppersGroups>> GetSearchAndCountDetails(string clientName, int minDays, int maxDays, string groupName)
        {
            var result = await _clientRepo.GetSearchAndCountDetails(clientName,minDays,maxDays,groupName);
            return result;
        }
        public async Task<List<AllShoppersGroups>> GetSearchAndCreateGroup(string clientName, int minDays, int maxDays, string groupName)
        {
            var result = await _clientRepo.GetSearchAndCreateGroup(clientName,minDays,maxDays,groupName);
            return result;
        }
        public async Task<int> AddGroupWithSearchDetails(string clientName, int minDays, int maxDays, string groupName)
        {
            var result = await _clientRepo.AddGroupWithSearchDetails(clientName, minDays, maxDays, groupName);
            return result;
        }
        public async Task<int> UploadShoppersToGroupsWithFile(string clientName, string groupName, string memberNumbers)
        {
            var result = await _clientRepo.UploadShoppersToGroupsWithFile(clientName,groupName, memberNumbers);
            return result;
        }
        public async Task<List<SearchCoupons>> FindCoupons(string clientName, int newsCategoryId, DateTime? valid, DateTime? expires)
        {
            var result = await _clientRepo.FindCoupons(clientName, newsCategoryId, valid, expires);
            return result;
        }
        public async Task<List<NewsCategories>> GetNewsCategories(string clientName)
        {
            var result = await _clientRepo.GetNewsCategories(clientName);
            return result;
        }
        public async Task<int> CreateBasketCoupon(string clientName, SaveBasketModel model)
        {
            var result = await _clientRepo.CreateBasketCoupon(clientName, model);
            return result;
        }
        public async Task<int> SaveUPCPromotion(string clientName, SaveUPCPromotionsModel model)
        {
            var result = await _clientRepo.SaveUPCPromotion(clientName, model);
            return (int)result;
        }
    }
}
