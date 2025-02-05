using Amazon.Runtime.Internal;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using RSASupportAPI.Models;
using RSASupportAPI.RSASupportBLL;
using SixLabors.Fonts;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace RSASupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSASupportAPIController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly Helpers _helpers;

        public RSASupportAPIController(
            IClientService clientService,
            Helpers helpers
            
            )
        {
            _clientService = clientService;
            _helpers = helpers;
        }
        [HttpGet]
        [Route("GetClients")]
        public async Task<ClinetResponse> GetClients()
        {
            ClinetResponse clinetResponse = new ClinetResponse();
            var result = await _clientService.GetClients();
            try
            {
                if (result == null) {
                    clinetResponse.ErroCode = "404";
                    clinetResponse.ErrorMessage = "Clients not found.";
                    return clinetResponse;
                }
                clinetResponse.ErroCode = "200";
                clinetResponse.ErrorMessage = "Success";
                clinetResponse.Clients = result;

            }
            catch (Exception ex) {
                clinetResponse.ErroCode = "500";
                clinetResponse.ErrorMessage = "Something went wrong. Please try again.";

            }
            return clinetResponse;
             
        }
        [HttpGet]
        [Route("GetClientData")]

        public async Task<IActionResult> GetClientData(string clientName, string selectQuery)
        {
           

            try
            {
                var result = await _clientService.GetClientData(clientName, selectQuery);
                if (result == null)
                {
                    return new JsonResult( new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Client data not found."
                        
                    });
                  
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ClientInfo = result
                });
            }
            catch (Exception ex) 
            { 
              return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetLMReward")]
        public async Task<IActionResult> GetLMReward(string clientName)
         {
            try
            {
                var result = await _clientService.GetLMReward(clientName);
                if (result == null)
                {
                    return new JsonResult(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "LM_Reward not found."

                    });

                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    LMRewards = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("FindShopper")]
        public async Task<IActionResult> GetFindShopperDetails(
            string clientName, string? email, string? memberNumber, string? mobileNumber, string? zipcode, int? clientStoreId, string? firstName, string? lastName, DateTime? signUpFromDate,
            DateTime? signUpToDate)
        {
            var result = await _clientService.GetFindShopperDetails(clientName, email, memberNumber, mobileNumber, zipcode, clientStoreId, firstName, lastName, signUpFromDate,signUpToDate);
            try
            {
                if (result == null || result.Count == 0)
                {
                    return new JsonResult(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Find shopper details not found."
                    });

                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    FindShopperInfo = result
                });
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),
                    FindShopperInfo = result
                });
            }

           // return Ok(result);
        }

        [HttpGet]
        [Route("GetClientStores")]
        public async Task<IActionResult> GetClientStores(string clientName)
        {
            
        
            try
            {
                var result = await _clientService.GetClientStores(clientName);
                if (result == null)
                {
                    return new JsonResult(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Client Stores not found."

                    });

                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ClientStores = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetRewardTypes")]
        public async Task<IActionResult> GetRewardTypes(string clientName)
        {
            var result = await _clientService.GetRewardTypes(clientName);
            try
            {
                if (result == null)
                {
                    return new JsonResult(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Reward types details not found."
                    });

                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Reward types details found.",
                    RewardTypes = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),
                    
                });
            }
            // return Ok(result);
        }
        [HttpGet]
        [Route("GetFindShopperDetailsById")]
        public async Task<IActionResult> GetFindShopperDetailsById(string clientName, int userDetailId)
        {
            var result = await _clientService.GetFindShopperDetailsById(clientName, userDetailId);
            try
            {
                if (result == null)
                {
                    return new JsonResult(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Find shopper details not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    FindShopperInfo = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpPut]
        [Route("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails(string clientName, int userDetailId, FindShopper findShopper)
        {
            var result = await _clientService.UpdateUserDetails(clientName, userDetailId, findShopper);
            try
            {
               if(result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Find shopper data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    FindShopper = result
                });
            }catch(Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }


        }
        [HttpGet]
        [Route("GetUserClipsAndRedemptionDates")]
        public async Task<IActionResult> GetUserClipsAndRedemptionDates(int userId)
        {
            var result = await _clientService.GetUserClipsAndRedemptionDates(userId);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User clips and redemptions dates data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status201Created,
                    ErrorMessage = "Successful",
                    UserClipsAndRedemptionDates = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("GetUserRewardCoupons")]
        public async Task<IActionResult> GetUserRewardCoupons(int userDetailId)
        {
            var result = await _clientService.GetUserRewardCoupons(userDetailId);

            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User reward  coupons not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    UserRewardCoupons = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetUserHistory")]
        public async Task<IActionResult> GetUserHistory(int userDetailId, string clientName)
        {
            var result = await _clientService.GetUserHistory(userDetailId, clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User history data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                     UserHistoryInfo= result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetUserBasketTransactions")]
        public async Task<IActionResult> GetUserBasketTransactions(int userId, string clientName)
        {
            var result = await _clientService.GetUserBasketTransactions(userId, clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User basket transaction data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    UserBasketTransactions = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetUserRecentPurchasedProducts")]
        //public async Task<IActionResult> GetUserRecentPurchasedProducts(int userId, int basketDataId, string clientName)
        public async Task<IActionResult> GetUserRecentPurchasedProducts(string Transaction, string clientName)

        {
            int userId = 0;
            int basketDataId = 0;
            userId = (Transaction != "" ? Convert.ToInt32(Transaction.Split("^")[0]) : 0); 
            basketDataId = (Transaction != "" ? Convert.ToInt32(Transaction.Split("^")[1]) : 0);
            var result = await _clientService.GetUserRecentPurchasedProducts(userId, basketDataId, clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User recent purchased products data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                   UserRecentProducts = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("UserPurchasedCoupon")]
        // public async Task<IActionResult> UserPurchasedCoupons(int userId, int basketDataId, string clientName)
        public async Task<IActionResult> UserPurchasedCoupons( string Transaction,string clientName)
        {
            int basketDataId = 0;
            int userId = 0;
            userId = (Transaction != "" ? Convert.ToInt32(Transaction.Split("^")[0]) : 0);
            basketDataId = (Transaction != "" ? Convert.ToInt32(Transaction.Split("^")[1]) : 0);
            var result = await _clientService.UserPurchasedCoupons(userId,basketDataId, clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User recent purchased coupons data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    UserPurchasedCoupons = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetMatchedRewardsForUser")]
        public async Task<IActionResult> GetMatchedRewardsForUser(string memberNumber, string clientName)
        {
            var result = await _clientService.GetMatchedRewardsForUser(memberNumber,clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User matched rewards data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    MatchedRewardsforUser = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("GetMyLMRewardsForUser")]
        public async Task<IActionResult> GetMyLMRewardsForUser(int lmRewardId, string clientName)
        {
            var result =await _clientService.GetMyLMRewardsForUser(lmRewardId, clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User LM Rewards data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    LMRewards = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("GetLMRewardsWithMemberNumber")]
        public async Task<IActionResult> GetLMRewardsForUserWithMemberNumber(string memberNumber,string clientName)
        {
            int lmRewardId = 0;
            var getLmReward = await _clientService.GetMatchedRewardsForUser(memberNumber, clientName); 
            if (getLmReward == null)
            {
                return BadRequest(new
                {
                    ErrorCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "User LM Rewards data not found."
                });
            } 
            lmRewardId =getLmReward[0]?.LMREWARDID ?? 0;
            var result = await _clientService.GetMyLMRewardsForUser(lmRewardId, clientName);
            try
            {
                if (result == null )
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User LM Rewards data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    LMRewards = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetMatchedRewardsForUserWithMemberNumber")]
        public async Task<IActionResult> GetMatchedRewardsForUserWithMemberNumber(string memberNumber, string clientName)
        {
            var result = await _clientService.GetMatchedRewardsForUserWithMemberNumber(memberNumber,clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Matched user rewards data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    MatchedLMRewards = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        //[HttpGet]
        //[Route("GetFindMemberDetails")]
        //public async Task<IActionResult> GetFindMemberDetails(int lmRewardId, int rewardTypeId, string clientName)
        //{
        //    var result = await _clientService.GetFindMemberDetails(lmRewardId, rewardTypeId, clientName);
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("GetFindMemberDetails")]
        public async Task<IActionResult> GetFindMemberDetails( int rewardTypeId, string memberNumber,string clientName)
        {
            int lmRewardId = 0;
            var getLmRewards = await _clientService.GetMatchedRewardsForUserWithMemberNumber(memberNumber,clientName);
            if (getLmRewards == null)
            {
               return  BadRequest(new
                {
                    ErrorCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "User LM Rewards data not found."
                });
            }
            lmRewardId = getLmRewards[0]?.LMREWARDID ?? 0;  
            var result = await _clientService.GetFindMemberDetails(lmRewardId, rewardTypeId, clientName);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User LM Rewards data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    FindMemberNumberDetails = result
                });
            }
            catch (Exception ex) {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpPost]
        [Route("SaveUserPoints")]
        public async Task<IActionResult> SaveUserPoints(string ClientName, string MemberNumber, string? UPC1, int? Qty1, string? UPC2, int? Qty2, int? TransactionAmount, int? RewardTypeId)
        {
            var result = await _clientService.SaveUserPoints(ClientName, MemberNumber, UPC1, Qty1, UPC2, Qty2, TransactionAmount, RewardTypeId);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed"
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    SaveUserPoints = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string clientName, DeleteUserRequest deleteUserRequest)
        {
            var result = await _clientService.DeleteUser(clientName, deleteUserRequest);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    DeleteUser = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetPaginationData")]
        public async Task<IActionResult> GetPaginationData(
            //List<FindShoppers> findShopperData,
            string clientName, int? page, int? limit, string? email, string? barcodeValue,
            string? mobile, string? zipCode, int? clientStoreId, string? firstName,
            string? lastName, DateTime? signUpFromDate, DateTime? signUpToDate, string? sortColumn, string? sortDirection)
        {
           var result = await _clientService.GetPaginationData(clientName, page, limit, email, barcodeValue, mobile, zipCode, clientStoreId, firstName, lastName, signUpFromDate, signUpToDate, sortColumn, sortDirection);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetFilteredUserDetails")]
        public async Task<IActionResult> GetFilteredUserDetails(string clientName, int? page, int? limit, string? sortColumn, string? sortDirection, string? searchTerm)
        {
            var result = await _clientService.GetFilteredUserDetails(clientName, page,limit, sortColumn, sortDirection, searchTerm);    
            return Ok(result);
        }

        [HttpGet("GetPaginate")]
        public async Task<IActionResult> GetPaginate(
    string clientName,
    string? email,
    string? memberNumber,
    string? mobileNumber,
    string? zipcode,
    int? clientStoreId,
    string? firstName,
    string? lastName,
    DateTime? signUpFromDate,
    DateTime? signUpToDate,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sortBy = "email",  // Sort field
    [FromQuery] bool isAscending = true,  // Sort direction
    [FromQuery] string? searchTerm = null  // Optional search term
)
        {
            // Step 1: Retrieve and filter data from the service
            var query = await _clientService.GetFindShopperDetails(clientName, email, memberNumber, mobileNumber, zipcode, clientStoreId, firstName, lastName, signUpFromDate, signUpToDate);

            // Check if data is null or empty
            if (query == null || query.Count == 0)
            {
                return NotFound("No records found.");
            }

           
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(item =>
                    (item.Email != null && item.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (item.BarCodeValue != null && item.BarCodeValue.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (item.Mobile != null && item.Mobile.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (item.ZipCode != null && item.ZipCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (item.FirstName != null && item.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (item.LastName != null && item.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (item.ClientStoreId.ToString().Contains(searchTerm)) ||
                    (item.SignUpDate.HasValue && item.SignUpDate.Value.ToString("MM/dd/yyyy").Contains(searchTerm))
                ).ToList();
            }
            
            query = sortBy.ToLower() switch
            {
                "email" => isAscending ? query.OrderBy(item => item.Email).ToList() : query.OrderByDescending(item => item.Email).ToList(),
                "membernumber" => isAscending ? query.OrderBy(item => item.BarCodeValue).ToList() : query.OrderByDescending(item => item.BarCodeValue).ToList(),
                "mobilenumber" => isAscending ? query.OrderBy(item => item.Mobile).ToList() : query.OrderByDescending(item => item.Mobile).ToList(),
                "zipcode" => isAscending ? query.OrderBy(item => item.ZipCode).ToList() : query.OrderByDescending(item => item.ZipCode).ToList(),
                "firstname" => isAscending ? query.OrderBy(item => item.FirstName).ToList() : query.OrderByDescending(item => item.FirstName).ToList(),
                "lastname" => isAscending ? query.OrderBy(item => item.LastName).ToList() : query.OrderByDescending(item => item.LastName).ToList(),
                "clientstoreid" => isAscending ? query.OrderBy(item => item.ClientStoreId).ToList() : query.OrderByDescending(item => item.ClientStoreId).ToList(),
                "signuptodate" => isAscending ? query.OrderBy(item => item.SignUpDate).ToList() : query.OrderByDescending(item => item.SignUpDate).ToList(),
                _ => isAscending ? query.OrderBy(item => item.Email).ToList() : query.OrderByDescending(item => item.Email).ToList()  // Default sort by email
            };

           
            var totalCount = query.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedData = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

         
            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Data = paginatedData
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateShopperGroups")]
     //   public async Task<IActionResult> CreateShopperGroups(string clientName, CreateShopperGroupModel model)
     public async Task<IActionResult> CreateShopperGroups(string clientName, string? SIGNFROMDATE, string? SIGNUPTODATE, string? FIRSTNAME, string? LASTNAME,
            string? USERNAME, string? ZIPCODE, int? STOREID, string? MEMBERNUMBER, string? GroupName, string? Description, int UserDetailId)
        {
            var result = await _clientService.CreateShopperGroups(clientName, SIGNFROMDATE, SIGNUPTODATE, FIRSTNAME, LASTNAME, USERNAME, ZIPCODE, STOREID, MEMBERNUMBER, GroupName, Description,UserDetailId);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ShopperGroups = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetUserGroups")]
        public async Task<IActionResult> GetUserGroups(string clientName, int userId)
        {
            var result = await _clientService.GetUserGroups(clientName, userId);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    UserGroups = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpGet]
        [Route("GetAvailableUserGroups")]
        public async Task<IActionResult> GetAvailableUserGroups(string clientName, int userId)
        {
            var result = await  _clientService.GetAvailableUserGroups(clientName, userId);
            try
            {
                if (result == null)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    AvailableUserGroups = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpPost]
        [Route("AddGroups")]
        public async Task<IActionResult> AddGroups(string clientName, int userId, int clubId)
        {
            var result = await _clientService.AddGroups(clientName,userId, clubId);
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    AddGroups = new
                    {
                        Message = "Successfully saved user groups.",
                        Count = result
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpPost]
        [Route("DeleteUserGroups")]
        public async Task<IActionResult> DeleteUserGroups(string clientName, int userId, int clubId)
        {
            var result = await _clientService.DeleteUserGroups(clientName, userId, clubId); 
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    DeleteUserGroups = new
                    {
                        Message = "Successfully saved user groups.",
                        Count = result
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpPost]
        [Route("CreateMembershipUser")]
        public async Task<IActionResult> CreateMembershipUser(string clientName, AspnetMembershipCreateUser aspnetMembershipCreateUser)
        {
            var result = await _clientService.CreateMembershipUser(clientName, aspnetMembershipCreateUser);

            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    User = new
                    {
                        Message = "Successfully created user.",
                        Count = result
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpPost]
        [Route("CreateShopper")]
        public async Task<IActionResult> CreateShopper(string clientName, CreateShopperModel model)
        {
            var result = await _clientService.CreateShopper(clientName, model);
            try
            {
                if(result.ErrorCode == "400")
                {
                    return BadRequest( new
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        Status = result.Status,
                    });
                }
              return  Ok( new
              {
                  ErrorCode = result.ErrorCode,
                  ErrorMessage = result.ErrorMessage,
                  Status = result.Status,
              });

                
            }
            catch (Exception ex) {
                return Ok(new
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message.ToString(),
                    Status = "Internel server error.",
                });

            }
        }
        [HttpGet]
        [Route("GetUserTypes")]
        public async Task<IActionResult> GetUserTypes(string clientName)
        {
           var result = await _clientService.GetUserTypes(clientName);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "User Types data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    UserTypes = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetAllRoles(string clientName)
        {
            var result = await _clientService.GetAllRoles(clientName);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Roles data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Roles = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("AllShopperGroups")]
        public async Task<IActionResult> GetAllShoppersGroups(string clientName, int groupId, int userId)
        {
            var result = await _clientService.GetAllShoppersGroups(clientName,groupId,userId);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                       ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "shoppers groups data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    AllShopperGroups = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }
        [HttpGet]
        [Route("GetAllTimeProducts")]
        public async Task<IActionResult> GetAllTimeProducts(string clientName, int GroupId)
        {
            var result = await _clientService.GetAllTimeProducts(clientName, GroupId);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "All time products data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                   AllTimeProducts = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpGet]
        [Route("GetTopProducts")]
        public async Task<IActionResult>  GetTopProducts(string clientName, int GroupId, int NoOfDays)
        {
            var result = await _clientService.GetTopProducts(clientName, GroupId, NoOfDays);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Top products data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    TopProducts = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("GetGroupAnalysisTimeLine")]
        public async Task<IActionResult> GetGroupAnalysisTimeLine(string clinetName, int groupId, int userId)
        {
            var result = await _clientService.GetGroupAnalysisTimeLine(clinetName, groupId, userId);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "GroupAnalysisTimeLine data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    GroupAnalysisList = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("DownloadShopperReport")]
        public async Task<IActionResult> DownloadUserShopperReport(string clientName,string groupName, int groupId)
        {
            var result = await _clientService.DownloadUserShopperReport(clientName, groupId);

            // Check if result contains data
            if (result != null && result.Count > 0)
            {
                // Generate timestamp for file name
                string timestamps = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                // Create an Excel workbook
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(timestamps);  // Use timestamp as sheet name

                // Add custom title in the first row
                worksheet.Cell(1, 1).Value = $"Users Report for {groupName}";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A1:C1").Merge(); // Merge cells to make the statement span multiple columns

                // Add a blank row below the title
                worksheet.Row(2).Height = 15;  // Optional: Adjust row height for the blank row

                // Set headers for the table in row 3
                worksheet.Cell(3, 1).Value = "UserName";
                worksheet.Cell(3, 2).Value = "BarcodeValue";
                worksheet.Cell(3, 3).Value = "StoreName";

                // Style the headers (bold and centered)
                worksheet.Row(3).Style.Font.Bold = true;
                worksheet.Row(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(3).Style.Fill.BackgroundColor = XLColor.LightGray;

                // Insert data starting from row 4
                var selectedRows = result.Select((row, i) =>
                    new
                    {
                        Index = i + 4, // Starting from row 4
                        row.UserName,
                        row.BarcodeValue,
                        row.StoreName
                    });

                // Insert rows using LINQ Select
                selectedRows.ToList().ForEach(s =>
                {
                    worksheet.Cell(s.Index, 1).Value = s.UserName;
                    worksheet.Cell(s.Index, 2).Value = s.BarcodeValue;
                    worksheet.Cell(s.Index, 3).Value = s.StoreName;
                });

                // Add borders to the entire table range (headers and data)
                var range = worksheet.Range(worksheet.Cell(3, 1), worksheet.Cell(result.Count + 3, 3)); // Include headers and data rows
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Adjust column widths to fit the content
                worksheet.Columns().AdjustToContents();

                // Save the workbook to a memory stream
                var stream = new MemoryStream();  // Do not use `using` here
                workbook.SaveAs(stream);

                // Reset the stream's position to the start before sending
                //stream.Position = 0;

               
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"GroupUsers_Report_{timestamps}.xls");
            }

            
            return NoContent();
        }

        [HttpPost]
        [Route("PreDefinedShopperGroupsByLastShoppedDate")]
        public async Task<IActionResult> PreDefinedShopperGroupsByLastShoppedDate(string clientName, PreDefinedShopperGroupsByLastShopped model)
        {
           var result = await _clientService.PreDefinedShopperGroupsByLastShoppedDate(clientName, model);
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Failed",

                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Status = "Ok"
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpPost]
        [Route("PreDefinedShopperGroupByZipcodesList")]
        public async Task<IActionResult> PreDefinedShopperGroupByZipcodesList(string clientName, PreDefinedShopperGroupByZipcodes model)
        {
            var result = await _clientService.PreDefinedShopperGroupByZipcodesList(clientName, model);
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Failed",

                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Status = "Ok"
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpPost]
        [Route("PreDefinedShopperGroupsByUPCList")]
        public async Task<IActionResult> PreDefinedShopperGroupsByUPCList(string clientName, PreDefinedShopperGroupsByUPCLists model)
        {
            var result = await _clientService.PreDefinedShopperGroupsByUPCList(clientName,model);
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Failed",

                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Status = "Ok"
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("GetProductCategories")]
        public async Task<IActionResult> GetProductCategories(string clientName)
        {
            var result = await _clientService.GetProductCategories(clientName);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Product categories data not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ProductsCategories = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }
        [HttpGet]
        [Route("GetTopShoppers")]
        public async Task<IActionResult> GetTopShoppers(string clientName, int noOfRecords, int storeId, string orderByDirection, DateTime? fromDate, DateTime? toDate, int? departmentId)
        {
           var result = await _clientService.GetTopShoppers(clientName,noOfRecords,storeId,orderByDirection,fromDate,toDate,departmentId);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Top shoppers details not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    TopShoppers = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("DownloadTopShoppers")]
        public async Task<IActionResult> DownloadTopShoppers(string clientName, int noOfRecords, int storeId, string orderByDirection, DateTime? fromDate, DateTime? toDate, int? departmentId)
        {
            var result = await _clientService.GetTopShoppers(clientName, noOfRecords,storeId,orderByDirection,fromDate,toDate,departmentId);

            // Check if result contains data
            if (result != null && result.Count > 0)
            {
                // Generate timestamp for file name
                string timestamps = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                // Create an Excel workbook
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(timestamps);  // Use timestamp as sheet name

                // Add custom title in the first row
                worksheet.Cell(1, 1).Value = $"Top Shoppers";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A1:E1").Merge(); // Merge cells to make the statement span multiple columns

                // Add a blank row below the title
               // worksheet.Row(2).Height = 15;  // Optional: Adjust row height for the blank row

                // Set headers for the table in row 3
                worksheet.Cell(2, 1).Value = "First Name";
                worksheet.Cell(2, 2).Value = "Last Name";
                worksheet.Cell(2, 3).Value = "Email";
                worksheet.Cell(2, 4).Value = "Store Name";
                worksheet.Cell(2, 5).Value = "Amount";

                // Style the headers (bold and centered)
                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(2).Style.Fill.BackgroundColor = XLColor.LightGray;

                // Insert data starting from row 4
                var selectedRows = result.Select((row, i) =>
                    new
                    {
                        Index = i + 3, // Starting from row 4
                        row.FIRSTNAME,
                        row.LASTNAME,
                        row.USERNAME,
                        row.STORENAME,
                        row.TOTALBASKETAMOUNT
                    });

                // Insert rows using LINQ Select
                selectedRows.ToList().ForEach(s =>
                {
                    worksheet.Cell(s.Index, 1).Value = s.FIRSTNAME;
                    worksheet.Cell(s.Index, 2).Value = s.LASTNAME;
                    worksheet.Cell(s.Index, 3).Value = s.USERNAME;
                    worksheet.Cell(s.Index, 4).Value = s.STORENAME;
                    worksheet.Cell(s.Index, 5).Value = $"{s.TOTALBASKETAMOUNT} $" ;
                });

                // Add borders to the entire table range (headers and data)
                var range = worksheet.Range(worksheet.Cell(2, 1), worksheet.Cell(result.Count + 2, 5)); // Include headers and data rows
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Adjust column widths to fit the content
                worksheet.Columns().AdjustToContents();

                // Save the workbook to a memory stream
                var stream = new MemoryStream();  // Do not use `using` here
                workbook.SaveAs(stream);

                // Reset the stream's position to the start before sending
                //stream.Position = 0;


                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Top Shoppers.xlsx");
            }


            return NoContent();
        }

        [HttpGet]
        [Route("GetAdvancedSearchShoppers")]
        public async Task<IActionResult> GetAdvancedSearchShoppers(string clientName, string? memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string? clubId, int? minBasketCount, int? maxBasketCount, string? storeId, int? minReedemCount, int? maxRedeemCount, string? departmentId, bool? isCreatedGroup)
        {
          var result = await _clientService.GetAdvancedSearchShoppers(clientName, memberNumber, transactionFrom, transactionTo, minSpend, maxSpend, clubId, minBasketCount, maxBasketCount, storeId, minReedemCount, maxRedeemCount, departmentId, isCreatedGroup);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetShoppersAdvancedSearch")]
        public async Task<IActionResult> GetShoppersAdvancedSearch(string memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string clubId, int? minBasketCount, int? maxBasketCount, string storeId, int? minReedemCount, int? maxRedeemCount, string departmentId, bool? isCreatedGroup)
        {
            var result = await _clientService.GetShoppersAdvancedSearch(memberNumber, transactionFrom, transactionTo, minSpend, maxSpend, clubId, minBasketCount, maxBasketCount, storeId, minReedemCount, maxRedeemCount, departmentId, isCreatedGroup);
            try
            {
                if (result == null || result.Count == 0)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Advanced shopper details not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    AdvancedShoppers = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }
        [HttpGet]
        [Route("DownloadAdvancedShoppers")]
        public async Task<IActionResult> DownloadAdvancedShoppers(string memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string clubId, int? minBasketCount, int? maxBasketCount, string storeId, int? minReedemCount, int? maxRedeemCount, string departmentId, bool? isCreatedGroup)
        {
          
            var result = await _clientService.GetShoppersAdvancedSearch(memberNumber, transactionFrom, transactionTo, minSpend, maxSpend, clubId, minBasketCount, maxBasketCount, storeId, minReedemCount, maxRedeemCount, departmentId, isCreatedGroup);
            if (result != null && result.Count > 0)
            {

                // create timestamps:
                string timestamps = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                // create excel workbook:
                using var workbook = new XLWorkbook();

                var worksheet =  workbook.Worksheets.Add(timestamps);

                // custom title for first row:
                worksheet.Cell(1, 1).Value = $"AdvancedSearch";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("A1:E1").Merge();

                // Add headers for the second row:
                worksheet.Cell(2, 1).Value = "First Name";
                worksheet.Cell(2, 2).Value = "Last Name";
                worksheet.Cell(2, 3).Value = "Email";
                worksheet.Cell(2, 4).Value = "Member Number";
                worksheet.Cell(2, 5).Value = "Preferred Store";

                // Add styles to headers
                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(2).Style.Fill.BackgroundColor = XLColor.White;

                

                // insert data from third row:
                var selectedRow = result.Select((row, i) =>
                {
                    return new
                    {
                        Index = i + 3,
                        row.FirstName,
                        row.LastName,
                        row.UserName,
                        row.loyaltyid,
                        row.PreferredStore
                    };
                });

                // inserted row data  using LINQ:
                selectedRow.ToList().ForEach(s =>

                {
                    worksheet.Cell(s.Index, 1).Value = s.FirstName;
                    worksheet.Cell(s.Index, 2).Value = s.LastName;
                    worksheet.Cell(s.Index, 3).Value = s.UserName;
                    worksheet.Cell(s.Index, 4).Value = s.loyaltyid;
                    worksheet.Cell(s.Index, 5).Value = s.PreferredStore;

                }
                );

                // Add borders headers and data :
                var range = worksheet.Range(worksheet.Cell(2, 1), worksheet.Cell(result.Count + 2, 5));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Adjust column width to fit to contents
                worksheet.Columns().AdjustToContents();

                // Save the workbook to memory stream:
                var stream = new MemoryStream();
                workbook.SaveAs(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"AdvancedSearch.xlsx");


            }
            return NoContent();

        }

        [HttpGet]
        [Route("GetFindShopperByUPCs")]
        public async Task<IActionResult> GetFindShopperByUPCs(string clientName, string? UPC, int NoOfCoupons, DateTime? FromDate, DateTime? ToDate)
        {
            var result = await _clientService.GetFindShopperByUPCs(clientName,UPC, NoOfCoupons, FromDate, ToDate);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Shoppers details not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    FindShopperByUPCsList = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetProductDetails")]
        public async Task<IActionResult> GetProductDetailsupc(string clientName, string? productCode, string? productName, int productCategoryId, bool? isMajorDepartment)
        {
            var result = await _clientService.GetProductDetailsupc(clientName,productCode, productName, productCategoryId, isMajorDepartment);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Product details not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ProductList = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetSearchAndCountDetails")]
        public async Task<IActionResult> GetSearchAndCountDetails(string clientName, int minDays, int maxDays, string groupName)
        {
            var result = await _clientService.GetSearchAndCountDetails(clientName, minDays, maxDays, groupName);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Shopper details not found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                     ShopperList= result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }
        [HttpPost]
        [Route("GetSearchAndCreateGroup")]
        public async Task<IActionResult> GetSearchAndCreateGroup(string clientName, int minDays, int maxDays, string groupName)
        {
            var result  = await _clientService.GetSearchAndCreateGroup(clientName, minDays, maxDays, groupName);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Failed"
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ShopperList = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpPost]
        [Route("AddGroupWithSearchDetails")]
        public async Task<IActionResult> AddGroupWithSearchDetails(string clientName, int minDays, int maxDays, string groupName)
        {
            var result = await _clientService.AddGroupWithSearchDetails(clientName, minDays, maxDays, groupName);
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Failed"
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    ShopperList = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }

        [HttpGet]
        [Route("UploadShoppersToGroups")]
        public async Task<IActionResult> UploadShoppersToGroups()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("UploadShoppersToGroups");
            worksheet.Cells[1, 1].Value = "MemberNumber";
            worksheet.Cells[2, 1].Value = "44010123456";
            worksheet.Column(1).AutoFit();
           var excelData = await package.GetAsByteArrayAsync();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadShoppersToGroups.xlsx");

        }
        [HttpGet]
        [Route("UploadUPCTempalate")]
        public async Task<IActionResult> UploadUPCTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("UploadUPCTemplate");
            worksheet.Cells[1, 1].Value = "UPCCodes";
            worksheet.Cells[2, 1].Value = "123638";
            worksheet.Column(1).AutoFit();
            var excelData = await package.GetAsByteArrayAsync();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadUPCTemplate.xlsx");
        }

        [HttpPost]
        [Route("UploadExcelFile")]
        public async Task<UploadResponse> UploadExcelFile(IFormFile file)
        {
           

            // Set license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var data = new List<Dictionary<string, string>>();
            var memberNumbers = new List<string>();

            using (var stream = new MemoryStream())
            {
                // Copy the uploaded file to memory stream
                await file.CopyToAsync(stream);

                // Make sure to reset the position of the stream to the beginning
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    // Check if the workbook contains any worksheets
                   

                    // Try to access the first worksheet
                    var worksheet = package.Workbook.Worksheets[0];

                    // Check if the worksheet is null
                    

                    // Get row and column count
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Read header
                    var header = new List<string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        header.Add(worksheet.Cells[1, col].Text); // Read header from first row
                    }

                    // Read rows
                    for (int row = 2; row <= rowCount; row++) // Start from second row
                    {
                        var rowData = new Dictionary<string, string>();

                        for (int col = 1; col <= colCount; col++)
                        {
                            rowData[header[col - 1]] = worksheet.Cells[row, col].Text; // Map data to header
                        }

                        data.Add(rowData);

                        if (rowData.ContainsKey("MemberNumber") && !string.IsNullOrEmpty(rowData["MemberNumber"]))
                        {
                            var memberValue = worksheet.Cells[row, header.IndexOf("MemberNumber") + 1].Value.ToString().Trim();
                            if (memberValue.Contains("E"))
                            {
                                memberValue = memberValue.TrimEnd('0').TrimEnd('.');
                            }
                            memberNumbers.AddRange(memberValue.Split(",").Select(u => u.Trim()));
                        }

                        if (rowData.ContainsKey("UPCCodes") && !string.IsNullOrEmpty(rowData["UPCCodes"]))
                        {
                            var memberValue = worksheet.Cells[row, header.IndexOf("UPCCodes") + 1].Value.ToString().Trim();
                            if (memberValue.Contains("E"))
                            {
                                memberValue = memberValue.TrimEnd('0').TrimEnd('.');
                            }
                            memberNumbers.AddRange(memberValue.Split(",").Select(u => u.Trim()));
                        }


                    }
                }
            }

            // Remove duplicates and join the values into a string
            var uniqueData = memberNumbers.Distinct().ToList();
            var result = string.Join(",", uniqueData);

            var uploadResponse = new UploadResponse()
            {
                FileJsonData = uniqueData.Select((u) => new FileJsonDataItem()
                {
                    UPCCodes = u
                }).ToList(),
                StringJsonFiledata = result
            };
            return uploadResponse;  
        }

       

        [HttpPost]
        [Route("UploadShoppers")]
        public async Task<IActionResult> UploadShoppers(IFormFile file, string clientName, string groupName)
        {

            Console.WriteLine(file);
            var fileJson = await UploadExcelFile(file);
            if (fileJson == null)
            {
                return BadRequest(new
                {
                    ErrorCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "No file uploaded."
                });
            }

          
            string memberNumbers = "";
            memberNumbers = fileJson.StringJsonFiledata;
            
            var result = await _clientService.UploadShoppersToGroupsWithFile(clientName, groupName, memberNumbers);


            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Failed"
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Upload = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("FindCoupons")]
        public async Task<IActionResult> FindCoupons(string clientName, int newsCategoryId, DateTime? valid, DateTime? expires)
        {
            var result = await _clientService.FindCoupons(clientName, newsCategoryId, valid, expires);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No coupons found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Coupons= result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpGet]
        [Route("GetNewsCategories")]
        public async Task<IActionResult> GetNewsCategories(string clientName)
        {
            var result = await _clientService.GetNewsCategories(clientName);
            try
            {
                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No categories found."
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    NewsCategories = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }
        }

        [HttpPost]
        [Route("CreateBasketCoupon")]
        public async Task<IActionResult> CreateBasketCoupon(string clientName, SaveBasketModel model)
        {
            var result = await _clientService.CreateBasketCoupon(clientName, model);
            try
            {
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Failed"
                    });
                }
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status200OK,
                    ErrorMessage = "Successful",
                    Status = result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message.ToString(),

                });
            }

        }
        [HttpPost]
        [Route("CreateUPCCoupon")]
        public async Task<IActionResult> SaveUPCPromotion(string clientName, SaveUPCPromotionsModel model)
        {
            var result = await _clientService.SaveUPCPromotion(clientName, model);
            return Ok(result);
        }
    }


}
