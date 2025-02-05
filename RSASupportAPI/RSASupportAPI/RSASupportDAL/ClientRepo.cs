using Amazon.S3;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Bytescout.BarCode;
using ClosedXML.Report.Options;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MoreLinq;
using Newtonsoft.Json.Linq;
using RSASupportAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.DirectoryServices;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace RSASupportAPI.RSASupportDAL
{
    public class ClientRepo : IClientRepo
    {
        private readonly List<AppConfigurations> _appConfigurations;
        private readonly Helpers _helpers;
        private readonly IAmazonS3 _amazonS3;
        private readonly S3BucketHelpers _s3BucketHelpers;
        private readonly IOptions<ConnectionStrings> _config;

        public ClientRepo(
            List<AppConfigurations> appConfigurations,
            Helpers helpers,
            IAmazonS3 amazonS3,
            S3BucketHelpers s3BucketHelpers,
            IOptions<ConnectionStrings> config
            )
        {
            _appConfigurations = appConfigurations;
            _helpers = helpers;
            _amazonS3 = amazonS3;
            _s3BucketHelpers = s3BucketHelpers;
            _config = config;
        }
        public async Task<Client> GetClients()
        {
            var client = new Client();

            //var bucketName = "rsasupportapi";
            //var s3FolderFilePath = "RSASupportConfigurations/AppConfigurations.json";
            //var validatePath = await _s3BucketHelpers.ExistedFolderPathKey(bucketName, s3FolderFilePath);
            //if (validatePath != null) {
            //    var getJsonData = await _s3BucketHelpers.S3JsonConfigData(bucketName, s3FolderFilePath);
            //    //var getJsonData = _appConfigurations.ToList();

            //    var getClientNames = getJsonData.Select(c => c.ClinetName
            //    ).ToList();

            //    client.clientNames = getClientNames;
            //}

            var getJsonData = await _s3BucketHelpers.GetS3JsonFileData();
            var getClientNames = getJsonData.Select(c => c.ClinetName
               ).ToList();

            client.clientNames = getClientNames;
            return await Task.FromResult(client);
        }
        public async Task<List<dynamic>> GetClientData(string clientName, string selectQuery)
        {

            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations != null && appconfigurations.Count > 0)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {

                    //string plaintext = "sai123";
                    //string encryptText = await _helpers.EncryptAes(plaintext);
                    //string textEncrypt = "JOzvfBDWRbcBMDk9QBDiyg==";
                    //string decrypt = await _helpers.DecryptAes(encryptText);
                    string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        Password = decryptText

                    };
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    //var row = await sqlHelpers.GetDynamicDataAsJson(selectQuery,null);


                    dynamic result = await sqlHelpers.GetDynamicQueryTableDataWithMultipleRows(selectQuery, null);



                    return result;


                }
            }
            return null;
        }

        public async Task<List<LMRewards>> GetLMRewards(string clientName)
        {
            List<LMRewards> lMRewards = new List<LMRewards>();

            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {



                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };
                    var storedProcName = "GET_LM_REWARD";
                    //var storedProcName = "GET_LM_REWARD";
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var getLmRewardsData = await sqlHelpers.GetMultipleRows(storedProcName, null);
                    lMRewards = (from dr in getLmRewardsData
                                 select (new LMRewards
                                 {
                                     ImageURL = dr["ImageURL"].ToString(),
                                     Title = dr["Title"].ToString(),
                                     RewardTitle = dr["RewardTitle"].ToString(),
                                     IsPointsBased = Convert.ToBoolean(dr["IsPointsBased"]),
                                     ValidFrom = Convert.ToDateTime(dr["ValidFrom"]).ToString("MM/dd/yyyy"),
                                     ExpiresOn = Convert.ToDateTime(dr["ExpiresOn"]).ToString("MM/dd/yyyy"),


                                 })).ToList();

                }

                return lMRewards;
            }
            return null;
        }

        public async Task<List<FindShoppers>> GetFindShopperDetails(
            string clientName,
             string? email,
            string? memberNumber,
            string? mobileNumber,
            string? zipcode,
            int? clientStoreId,
            string? firstName,
            string? lastName,
            DateTime? signUpFromDate,
            DateTime? signUpToDate
            )
        {
            var findShoppers = new List<FindShoppers>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };
                    // var storedProcName = "GetUserDetails";
                    var storedProcName = "GetUserInfo";
                    int clientstoreId = clientStoreId ?? 0;
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Email",string.IsNullOrEmpty(email) ? DBNull.Value : email ),
                        new SqlParameter("@BarCodeValue",string.IsNullOrEmpty(memberNumber) ? DBNull.Value : memberNumber),
                        new SqlParameter("@Mobile",string.IsNullOrEmpty(mobileNumber) ? DBNull.Value : mobileNumber),
                        new SqlParameter("@ZipCode",string.IsNullOrEmpty(zipcode) ? DBNull.Value :zipcode ),
                        new SqlParameter("@ClientStoreId",clientStoreId.HasValue ? (object)clientStoreId.Value : DBNull.Value) ,
                        new SqlParameter("@FirstName",string.IsNullOrEmpty(firstName) ?DBNull.Value : firstName),
                        new SqlParameter("@LastName",string.IsNullOrEmpty(lastName) ? DBNull.Value : lastName),
                        new SqlParameter("@SignUpFromDate",signUpFromDate.HasValue ? (object)signUpFromDate.Value : DBNull.Value),
                        new SqlParameter("@SignUpToDate",signUpToDate.HasValue ? (object)signUpToDate.Value : DBNull.Value)
                    };
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);
                    if (result != null)
                    {
                        findShoppers = (from dr in result
                                        select (new FindShoppers
                                        {
                                            UserDetailId = Convert.ToInt32(dr["UserDetailId"]),
                                            Email = dr["Email"].ToString() ?? string.Empty,
                                            BarCodeValue = dr["BarCodeValue"].ToString() ?? string.Empty,
                                            Mobile = dr["Mobile"].ToString() ?? string.Empty,
                                            ZipCode = dr["ZipCode"].ToString() ?? string.Empty,
                                            ClientStoreId = dr["ClientStoreId"] != DBNull.Value ? Convert.ToInt32(dr["ClientStoreId"]) : (int?)null,
                                            FirstName = dr["FirstName"].ToString() ?? string.Empty,
                                            LastName = dr["LastName"].ToString() ?? string.Empty,
                                            SignUpDate = dr["SignUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["SignUpDate"]) : (DateTime?)null,
                                        })
                                        ).ToList();
                        return findShoppers;
                    }





                }


            }
            return null;
        }

        public async Task<List<ClientStores>> GetClientStores(string clientName)
        {
            var clienStores = new List<ClientStores>();

            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };
                    // var storedProcName = "GetUserDetails";
                    var storedProcName = "Get_Client_Stores";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, null);
                    clienStores = (from dr in result
                                   select (new ClientStores
                                   {
                                       ClientStoreId = Convert.ToInt32(dr["ClientStoreId"]),
                                       StoreName = dr["StoreName"].ToString() ?? string.Empty,
                                   })).ToList();
                }


            }
            return clienStores;

        }

        public async Task<List<RewardTypes>> GetRewardTypes(string clientName)
        {
            var rewardTypes = new List<RewardTypes>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };
                    // var storedProcName = "GetUserDetails";
                    var storedProcName = "GetLMRewardTypes";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, null);
                    if (result != null)
                    {
                        rewardTypes = (from dr in result
                                       select (new RewardTypes
                                       {
                                           RewardType = Convert.ToInt32(dr["RewardType"]),
                                           Description = dr["Description"].ToString() ?? string.Empty,
                                       })).ToList();
                        return rewardTypes;
                    }

                }

            }
            return null;
        }

        public async Task<FindShopper> GetFindShopperDetailsById(string clientName, int userDetailId)
        {
            FindShopper findShopper = null;
            findShopper = new FindShopper();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };

                    var storedProcName = "GetByIdUserInfo";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var userDetailIdParameter = new SqlParameter[]
                    {
                        new SqlParameter("@UserDetailId",userDetailId)
                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetSingleRow(storedProcName, userDetailIdParameter);
                    if (result != null)
                    {
                        findShopper.Email = result["Email"] != DBNull.Value ? result["Email"].ToString() : string.Empty;
                        findShopper.BarCodeValue = result["BarCodeValue"] != DBNull.Value ? result["BarCodeValue"].ToString() : string.Empty;
                        findShopper.Mobile = result["Mobile"] != DBNull.Value ? result["Mobile"].ToString() : string.Empty;
                        findShopper.ZipCode = result["ZipCode"] != DBNull.Value ? result["ZipCode"].ToString() : string.Empty;
                        findShopper.ClientStoreId = result["ClientStoreId"] != DBNull.Value ? Convert.ToInt32(result["ClientStoreId"]) : 0;
                        findShopper.FirstName = result["FirstName"] != DBNull.Value ? result["FirstName"].ToString() : string.Empty;
                        findShopper.LastName = result["LastName"] != DBNull.Value ? result["LastName"].ToString() : string.Empty;
                        findShopper.SignUpDate = result["SignUpDate"] != DBNull.Value ? Convert.ToDateTime(result["SignUpDate"]) : null;
                        return findShopper;
                    }



                }

            }


            return null;
        }

        public async Task<FindShopper> UpdateUserDetails(string clientName, int userDetailId, FindShopper findShopper)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };

                    var storedProcName = "UpdateUserDetails";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserDetailId",userDetailId),
                        new SqlParameter("@Email",findShopper.Email),
                        new SqlParameter("@BarCodeValue",findShopper.BarCodeValue),
                        new SqlParameter("@Mobile",findShopper.Mobile),
                        new SqlParameter("@ZipCode",findShopper.ZipCode),
                        new SqlParameter("@ClientStoreId",findShopper.ClientStoreId),
                        new SqlParameter("@FirstName",findShopper.FirstName),
                        new SqlParameter("@LastName",findShopper.LastName),
                        new SqlParameter("@SignUpDate",findShopper.SignUpDate)
                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsAffected = await sqlHelpers.UpdateTable(storedProcName, parameters);
                    if (rowsAffected != 0)
                    {
                        return findShopper;
                    }



                }

            }
            return null;
        }

        public async Task<List<UserClipsAndRedemptionDates>> GetUserClipsAndRedemptionDates(int userId)
        {
            List<UserClipsAndRedemptionDates> userClipsAndRedemptionDates = new List<UserClipsAndRedemptionDates>();

            var connectionString = _config.Value.RSAGroceryDBCon.ToString();

            string storedProcedureName = "PROC_CUSTOM_GET_USER_CLIPS_REDEMPTION_DATES";
            var userIdParameter = new SqlParameter[]
            {
                        new SqlParameter("@USERID",userId)
            };
            SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
            var result = await sqlHelpers.GetMultipleRows(storedProcedureName, userIdParameter);
            if (result.Count > 0)
            {
                userClipsAndRedemptionDates = (from dr in result
                                               select (new UserClipsAndRedemptionDates
                                               {
                                                   NEWSID = Convert.ToInt32(dr["NEWSID"]),
                                                   TITLE = dr["TITLE"].ToString() ?? string.Empty,
                                                   DETAILS = dr["DETAILS"].ToString() ?? string.Empty,
                                                   OTHERDETAILS = dr["OTHERDETAILS"].ToString() ?? string.Empty,
                                                   VALIDFROMDATE = dr["VALIDFROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["VALIDFROMDATE"]).ToString("MM/dd/yyy") : "",
                                                   EXPIRESON = dr["EXPIRESON"] != DBNull.Value ? Convert.ToDateTime(dr["VALIDFROMDATE"]).ToString("MM/dd/yyy") : "",
                                                   NEWSCATEGORYID = dr["NEWSCATEGORYID"] != DBNull.Value ? Convert.ToInt32(dr["NEWSCATEGORYID"]) : (int?)null,
                                                   BRANDNAME = dr["BRANDNAME"].ToString() ?? string.Empty,
                                                   NCRIMPRESSIONDATE = dr["NCRIMPRESSIONDATE"] != DBNull.Value ? Convert.ToDateTime(dr["VALIDFROMDATE"]).ToString("MM/dd/yyy HH:mm:ss") : "",
                                                   REDEMPTIONDATE = dr["REDEMPTIONDATE"] != DBNull.Value ? Convert.ToDateTime(dr["REDEMPTIONDATE"]).ToString("MM/dd/yyyy") : ""
                                               })).ToList();
                return userClipsAndRedemptionDates;
            }
            return null;
        }

        public async Task<List<UserRewardCoupons>> GetUserRewardCoupons(int userDetailId)
        {
            List<UserRewardCoupons> userRewardCoupons = new List<UserRewardCoupons>();
            var connectionString = _config.Value.RSAGroceryDBCon.ToString();

            string storedProcedureName = "PROC_CUSTOM_GET_USER_REWARD_COUPONS";
            var userDetailIdParameter = new SqlParameter[]
            {
                        new SqlParameter("@USERDETAILID",userDetailId)
            };
            SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
            var result = await sqlHelpers.GetMultipleRows(storedProcedureName, userDetailIdParameter);
            if (result.Count > 0)
            {
                userRewardCoupons = (from dr in result
                                     select (new UserRewardCoupons
                                     {
                                         TITLE = dr["TITLE"].ToString() ?? string.Empty,
                                         LM_REWARD_ID = Convert.ToInt32(dr["LM_REWARD_ID"]),
                                         APPLIEDDATE = dr["APPLIEDDATE"] != DBNull.Value ? Convert.ToDateTime(dr["APPLIEDDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                         CREATEDDATE = dr["CREATEDDATE"] != DBNull.Value ? Convert.ToDateTime(dr["CREATEDDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                         USERDETAILID = dr["USERDETAILID"] != DBNull.Value ? Convert.ToInt32(dr["USERDETAILID"]) : 0,
                                         MEMBERNUMBER = dr["MEMBERNUMBER"].ToString() ?? string.Empty
                                     })).ToList();

                return userRewardCoupons;
            }
            return null;
        }

        public async Task<List<PROC_CUSTOM_VIEW_USER_HISTORY>> GetUserHistory(int userDetailId, string clientName)
        {

            List<PROC_CUSTOM_VIEW_USER_HISTORY> userHistory = new List<PROC_CUSTOM_VIEW_USER_HISTORY>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };

                    var storedProcName = "PROC_CUSTOM_VIEW_USER_HISTORY";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserId",userDetailId),

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);
                    if (result.Count > 0)
                    {

                        userHistory = (from dr in result
                                       select new PROC_CUSTOM_VIEW_USER_HISTORY
                                       {
                                           UserDetailId = dr["UserDetailId"] != DBNull.Value ? Convert.ToInt32(dr["UserDetailId"]) : 0,
                                           FirstName = dr["FirstName"].ToString() ?? string.Empty,
                                           LastName = dr["LastName"].ToString() ?? string.Empty,
                                           UserName = dr["UserName"].ToString() ?? string.Empty,
                                           Mobile = dr["Mobile"].ToString() ?? string.Empty,
                                           ZipCode = dr["ZipCode"].ToString() ?? string.Empty,
                                           BarcodeValue = dr["BarcodeValue"].ToString() ?? string.Empty,
                                           CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.MinValue,
                                           TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TotalAmount"]) : 0,
                                           UserRank = dr["UserRank"] != DBNull.Value ? Convert.ToInt32(dr["UserRank"]) : 0,
                                           UserClubNames = dr["UserClubNames"].ToString() ?? string.Empty,
                                           TotalbasketCount = dr["TotalbasketCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalbasketCount"]) : 0,
                                           AvgBasketsAmount = dr["AvgBasketsAmount"] != DBNull.Value ? Convert.ToDecimal(dr["AvgBasketsAmount"]) : 0,
                                           Clips = dr["CLips"] != DBNull.Value ? Convert.ToInt32(dr["CLips"]) : 0,
                                           Redemptions = dr["Redemptions"] != DBNull.Value ? Convert.ToInt32(dr["Redemptions"]) : 0
                                       }).ToList();

                        return userHistory;

                    }



                }

            }

            return null;
        }

        public async Task<List<UserBasketTransactions>> GetUserBasketTransactions(int userId, string clientName)
        {

            var viewUser = await GetUserHistory(userId, clientName);

            List<UserBasketTransactions> userBasketTransactions = null;
            userBasketTransactions = new List<UserBasketTransactions>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_USER_BASKETTRANSACTIONS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserId",userId),

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0 && viewUser.Count > 0)
                    {
                        userBasketTransactions = (from dr in result
                                                  select (
                                                  new UserBasketTransactions
                                                  {
                                                      UserId = viewUser[0].UserDetailId,
                                                      BasketDataID = dr["BasketDataID"] != DBNull.Value ? Convert.ToInt32(dr["BasketDataID"]) : 0,
                                                      LoyaltyId = dr["LoyaltyId"].ToString() ?? string.Empty,
                                                      Storeid = dr["Storeid"] != DBNull.Value ? Convert.ToInt32(dr["Storeid"]) : 0,
                                                      TransactionDate = dr["TransactionDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransactionDate"]).ToString("MM/dd/yyyy") : "",
                                                      TotalBasketAmount = dr["TotalBasketAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TotalBasketAmount"]) : 0,
                                                      StoreName = dr["StoreName"].ToString() ?? string.Empty,
                                                      RowNumber = dr["RowNumber"] != DBNull.Value ? Convert.ToInt64(dr["RowNumber"]) : null,
                                                      DropDownName = Convert.ToDateTime(dr["TransactionDate"]).ToString("MM/dd/yyy") + " " + dr["StoreName"].ToString() + " " + (dr["TotalBasketAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TotalBasketAmount"]) : 0),
                                                      DropDownKey = viewUser[0].UserDetailId + "^" + (dr["BasketDataID"] != DBNull.Value ? Convert.ToInt32(dr["BasketDataID"]) : 0)
                                                  }
                                                  )).ToList();


                        return userBasketTransactions;
                    }



                }

            }

            return null;
        }

        public async Task<List<GetUserRecentPurchasedProducts>> GetUserRecentPurchasedProducts(int userId, int basketDataId, string clientName)
        {
            List<GetUserRecentPurchasedProducts> getUserRecentPurchasedProducts = new List<GetUserRecentPurchasedProducts>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_USER_RECENT_PURCHASED_PRODUCTS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserId",userId),
                         new SqlParameter("@BasketDataId",basketDataId),

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        getUserRecentPurchasedProducts = (from dr in result
                                                          select (new GetUserRecentPurchasedProducts
                                                          {
                                                              ProductName = dr["ProductName"].ToString() ?? string.Empty,
                                                              ProductCode = dr["ProductCode"].ToString() ?? string.Empty,
                                                              Qty = dr["Qty"] != DBNull.Value ? Convert.ToInt32(dr["Qty"]) : 0,
                                                              Amount = dr["Amount"] != DBNull.Value ? Convert.ToDecimal(dr["Amount"]) : 0
                                                          })).ToList();

                        return getUserRecentPurchasedProducts;
                    }



                }

            }

            return null;
        }

        public async Task<List<UserPurchasedCoupons>> UserPurchasedCoupons(int userId, int basketDataId, string clientName)
        {
            List<UserPurchasedCoupons> userPurchasedCoupons = new List<UserPurchasedCoupons>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_USER_RECENT_PURCHASED_COUPONS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserId",userId),
                         new SqlParameter("@BasketDataId",basketDataId),

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        userPurchasedCoupons = (from dr in result
                                                select (new UserPurchasedCoupons
                                                {
                                                    Title = dr["Title"].ToString() ?? string.Empty,
                                                    Details = dr["Details"].ToString() ?? string.Empty,
                                                    NewsId = dr["NewsId"] != DBNull.Value ? Convert.ToInt32(dr["NewsId"]) : 0,
                                                    Value = dr["Value"] != DBNull.Value ? Convert.ToDecimal(dr["Value"]) : 0
                                                })).ToList();

                        return userPurchasedCoupons;
                    }



                }

            }
            return null;
        }

        public async Task<List<MatchRewards>> GetMatchedRewardsForUser(string memberNumber, string clientName)
        {
            List<MatchRewards> matchRewards = new List<MatchRewards>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "LM_GetMatchedRewardsForUser";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MemberNumber",memberNumber),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        matchRewards = (from dr in result
                                        select (new MatchRewards
                                        {
                                            LMREWARDID = Convert.ToInt32(dr["LM_REWARD_ID"]),
                                            RewardTypeID = Convert.ToInt32(dr["RewardTypeID"]),
                                            BuyQtyAmount = Convert.ToInt32(dr["BuyQtyAmount"]),
                                            RewardQtyAmount = Convert.ToInt32(dr["RewardQtyAmount"]),
                                            ValidFrom = Convert.ToDateTime(dr["ValidFrom"]).ToString("MM/dd/yyyy"),
                                            ExpiresOn = Convert.ToDateTime(dr["ExpiresOn"]).ToString("MM/dd/yyyy"),

                                        })).ToList();

                        return matchRewards;
                    }



                }

            }
            return null;
        }

        public async Task<List<MyLMRewards>> GetMyLMRewardsForUser(int lmRewardId, string clientName)
        {
            List<MyLMRewards> myLMRewards = new List<MyLMRewards>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GET_MY_REWARDS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@LM_REWARD_ID",lmRewardId),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        myLMRewards = (from dr in result
                                       select (new MyLMRewards
                                       {
                                           ImageURL = dr["ImageURL"].ToString() ?? string.Empty,
                                           BuyQtyAmount = Convert.ToInt32(dr["BuyQtyAmount"]),
                                           RewardQtyAmount = Convert.ToInt32(dr["RewardQtyAmount"]),
                                           RewardQtyAmountMoney = Convert.ToDecimal(dr["RewardQtyAmountMoney"]),
                                           PointsPerEach = Convert.ToDecimal(dr["PointsPerEach"]),
                                           StringFormattedPoints = Convert.ToDecimal(dr["PointsPerEach"]).ToString("N0")
                                       })).ToList();

                        return myLMRewards;
                    }



                }

            }
            return null;
        }

        public async Task<List<MatchRewards>> GetMatchedRewardsForUserWithMemberNumber(string memberNumber, string clientName)
        {
            List<MatchRewards> matchRewards = new List<MatchRewards>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "LM_GetMatchedRewardsForUserWithMemberNumber";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MemberNumber",memberNumber),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        matchRewards = (from dr in result
                                        select (new MatchRewards
                                        {
                                            LMREWARDID = Convert.ToInt32(dr["LM_REWARD_ID"]),
                                            RewardTypeID = Convert.ToInt32(dr["RewardTypeID"]),
                                            BuyQtyAmount = Convert.ToInt32(dr["BuyQtyAmount"]),
                                            RewardQtyAmount = Convert.ToInt32(dr["RewardQtyAmount"]),
                                            ValidFrom = Convert.ToDateTime(dr["ValidFrom"]).ToString("MM/dd/yyyy"),
                                            ExpiresOn = Convert.ToDateTime(dr["ExpiresOn"]).ToString("MM/dd/yyyy"),
                                            RewardQtyAmountMoney = Convert.ToDecimal(dr["RewardQtyAmountMoney"]),
                                            PointsPerEach = Convert.ToDecimal(dr["PointsPerEach"])

                                        })).ToList();

                        return matchRewards;
                    }



                }

            }
            return null;
        }

        public async Task<List<FindMemberNumber>> GetFindMemberDetails(int lmRewardId, int rewardTypeId, string clientName)
        {
            List<FindMemberNumber> findMemberNumbers = new List<FindMemberNumber>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GET_LM_REWARD_DETAILS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@LM_REWARD_ID",lmRewardId),
                        new SqlParameter("@RewardTypeID",rewardTypeId),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        findMemberNumbers = (from dr in result
                                             select (new FindMemberNumber
                                             {
                                                 ValidFrom = Convert.ToDateTime(dr["ValidFrom"]).ToString("MM/dd/yyyy"),
                                                 ExpiresOn = Convert.ToDateTime(dr["ExpiresOn"]).ToString("MM/dd/yyyy"),
                                                 Title = dr["Title"].ToString() ?? string.Empty,

                                                 RewardTypeID = Convert.ToInt32(dr["RewardTypeID"]),
                                                 BuyQtyAmount = Convert.ToInt32(dr["BuyQtyAmount"]),
                                                 RewardQtyAmount = Convert.ToInt32(dr["RewardQtyAmount"]),
                                                 RewardTitle = dr["RewardTitle"].ToString() ?? string.Empty,
                                                 AdditionalDetails = dr["AdditionalDetails"].ToString() ?? string.Empty,
                                                 POSDetails = dr["POSDetails"].ToString() ?? string.Empty,
                                                 ImageURL = dr["ImageURL"].ToString() ?? string.Empty,
                                                 CreatedUserID = Convert.ToInt32(dr["CreatedUserID"]),
                                                 CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]).ToString("MM/dd/yyyy"),

                                                 RewardGroupID = Convert.ToInt32(dr["RewardGroupID"]),
                                                 CouponID = Convert.ToInt32(dr["CouponID"]),
                                                 RewardStatus = Convert.ToInt32(dr["RewardStatus"]),
                                                 RewardDepartmentID = Convert.ToInt32(dr["RewardDepartmentID"]),
                                                 RewardMustBeUsedByDate = Convert.ToDateTime(dr["RewardMustBeUsedByDate"]).ToString("MM/dd/yyyy"),
                                                 IsTargetSpecific = Convert.ToBoolean(dr["IsTargetSpecific"]),
                                                 IsDiscountPercentage = Convert.ToBoolean(dr["IsDiscountPercentage"]),
                                                 PointsPerEach = Convert.ToDecimal(dr["PointsPerEach"]),
                                                 RewardCouponMinQty = Convert.ToInt32(dr["RewardCouponMinQty"]),
                                                 RewardCouponTypeID = Convert.ToInt32(dr["RewardCouponTypeID"]),
                                                 RewardQtyAmountMoney = Convert.ToDecimal(dr["RewardQtyAmountMoney"]),
                                                 IsDepartmentSpecific = Convert.ToBoolean(dr["IsDepartmentSpecific"]),
                                                 IsStoreSpecific = Convert.ToBoolean(dr["IsStoreSpecific"]),
                                                 IsPointsBased = Convert.ToBoolean(dr["IsPointsBased"]),
                                                 TierValue = Convert.ToInt32(dr["TierValue"]),
                                                 NumberOfVisits = Convert.ToInt32(dr["NumberOfVisits"]),

                                             })).ToList();

                        return findMemberNumbers;
                    }



                }

            }
            return null;
        }

        public async Task<UserPoints> SaveUserPoints(string ClientName, string MemberNumber, string? UPC1, int? Qty1, string? UPC2, int? Qty2, int? TransactionAmount, int? RewardTypeId)
        {
            UserPoints userPoints = new UserPoints();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == ClientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "SAVE_USER_POINTS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MemberNumber",MemberNumber ?? string.Empty),
                        new SqlParameter("@UPC1",string.IsNullOrEmpty(UPC1)  ? string.Empty : UPC1 ),
                        new SqlParameter("@UPC2",string.IsNullOrEmpty(UPC2)  ? string.Empty : UPC2),
                        new SqlParameter("@QTY1",string.IsNullOrEmpty(Qty1.ToString() ) ? string.Empty :Qty1 ),
                        new SqlParameter("@QTY2",string.IsNullOrEmpty(Qty2.ToString() ) ? string.Empty :Qty2),
                        new SqlParameter("@TransactionTotalAmount",string.IsNullOrEmpty(TransactionAmount.ToString()) ? string.Empty : TransactionAmount),
                        new SqlParameter("@Type",RewardTypeId)



                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsAffected = await sqlHelpers.InsertTable(storedProcName, parameters);

                    if (rowsAffected == -1)
                    {
                        userPoints.MemberNumber = MemberNumber;
                        userPoints.UPC1 = UPC1;
                        userPoints.QTY1 = Qty1;
                        userPoints.QTY2 = Qty2;
                        userPoints.UPC2 = UPC2;
                        userPoints.TransactionTotalAmount = TransactionAmount;
                        userPoints.Type = RewardTypeId;
                        return userPoints;
                    }





                }

            }
            return null;
        }

        public async Task<DeleteUserRequest> DeleteUser(string clientName, DeleteUserRequest deleteUserRequest)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "DeleteMobileUser";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@UserId",deleteUserRequest.UserDetailId),
                        new SqlParameter("@MemberNumber",deleteUserRequest.MemberNumber ?? string.Empty),




                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsAffected = await sqlHelpers.UpdateTable(storedProcName, parameters);

                    if (rowsAffected > 0)
                    {

                        return deleteUserRequest;
                    }





                }

            }
            return null;
        }

        // Fetching shopperData, filter,sorting,pagining
        //
        //  public async Task<FindShopperPagination> GetPaginationData(
        //string clientName, int? page, int? limit, string? email, string? barcodeValue,
        //string? mobile, string? zipCode, int? clientStoreId, string? firstName,
        //string? lastName, DateTime? signUpFromDate, DateTime? signUpToDate, string? sortColumn, string? sortDirection)
        //  {
        //      var findShopperPagination = new FindShopperPagination
        //      {
        //          FindShopperList = new List<FindShoppers>()
        //      };

        //      var appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
        //      if (appconfigurations?.Count > 0)
        //      {
        //          var s3bucketData = appconfigurations.FirstOrDefault(c => c.ClinetName == clientName);
        //          if (s3bucketData != null)
        //          {
        //              var password = s3bucketData.ClinetName == "Veritra RSA"
        //                  ? s3bucketData.Password
        //                  : await _helpers.DecryptAes(s3bucketData.Password);

        //              var rsaDBCon = new RSADBConnection
        //              {
        //                  DataSource = s3bucketData.DBInstanceName,
        //                  Database = s3bucketData.DBName,
        //                  UserId = s3bucketData.UserName,
        //                  Password = password
        //              };

        //              Get FindShopper search data:
        //              var findShoppers = await GetFindShopperDetails(clientName, email, barcodeValue, mobile, zipCode, clientStoreId, firstName, lastName, signUpFromDate, signUpToDate);

        //              Filter data
        //              var filterShopperData = findShoppers.Where(shopper =>
        //                  (string.IsNullOrEmpty(email) || shopper.Email.Contains(email)) &&
        //                  (string.IsNullOrEmpty(barcodeValue) || shopper.BarCodeValue.Contains(barcodeValue)) &&
        //                  (string.IsNullOrEmpty(mobile) || shopper.Mobile.Contains(mobile)) &&
        //                  (string.IsNullOrEmpty(zipCode) || shopper.ZipCode.Contains(zipCode)) &&
        //                  (!clientStoreId.HasValue || shopper.ClientStoreId == clientStoreId) &&
        //                  (string.IsNullOrEmpty(firstName) || shopper.FirstName.Contains(firstName)) &&
        //                  (string.IsNullOrEmpty(lastName) || shopper.LastName.Contains(lastName)) &&
        //                  (!signUpFromDate.HasValue || shopper.SignUpDate >= signUpFromDate) &&
        //                  (!signUpToDate.HasValue || shopper.SignUpDate <= signUpToDate)
        //              ).ToList();

        //              Sort data
        //              if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
        //              {
        //                  filterShopperData = sortDirection.ToLower() == "asc"
        //                      ? filterShopperData.OrderBy(e => GetPropertyValue(e, sortColumn)).ToList()
        //                      : filterShopperData.OrderByDescending(e => GetPropertyValue(e, sortColumn)).ToList();
        //              }

        //              Pagination
        //              int totalRecords = filterShopperData.Count;
        //              int pageSize = limit ?? 5;
        //              int currentPage = page ?? 1;
        //              int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        //              var paginationResult = filterShopperData
        //                  .Skip((currentPage - 1) * pageSize)
        //                  .Take(pageSize)
        //                  .ToList();

        //              Return pagination result
        //              return new FindShopperPagination
        //              {
        //                  FindShopperList = paginationResult,
        //                  SearchFindShopper = filterShopperData, // Optional: Entire filtered data if needed
        //                  TotalCount = totalRecords,
        //                  TotalPages = totalPages,

        //              };
        //          }
        //      }
        //      return null;
        //  }

        //  private object GetPropertyValue(FindShoppers shopper, string propertyName)
        //  {
        //      return typeof(FindShoppers).GetProperty(propertyName)?.GetValue(shopper, null) ?? string.Empty;
        //  }

        public async Task<FindShopperPagination> GetPaginationData(
            string clientName, int? page, int? limit, string? email, string? barcodeValue,
            string? mobile, string? zipCode, int? clientStoreId, string? firstName,
            string? lastName, DateTime? signUpFromDate, DateTime? signUpToDate, string? sortColumn, string? sortDirection)
        {
            FindShopperPagination findShopperPagination = new FindShopperPagination();
            findShopperPagination.FindShopperList = new List<FindShoppers>();

            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "pagination_data";
                    //var storedProcName = "search_records_without_limit";
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlParameter[] parameters = new SqlParameter[]
         {
            new SqlParameter("@Page", page),
            new SqlParameter("@Limit", limit),
           new SqlParameter("@Email",string.IsNullOrEmpty(email) ? DBNull.Value : email ),
                        new SqlParameter("@BarCodeValue",string.IsNullOrEmpty(barcodeValue) ? DBNull.Value : barcodeValue),
                        new SqlParameter("@Mobile",string.IsNullOrEmpty(mobile) ? DBNull.Value : mobile),
                        new SqlParameter("@ZipCode",string.IsNullOrEmpty(zipCode) ? DBNull.Value :zipCode ),
                        new SqlParameter("@ClientStoreId",clientStoreId.HasValue ? (object)clientStoreId.Value : DBNull.Value) ,
                        new SqlParameter("@FirstName",string.IsNullOrEmpty(firstName) ?DBNull.Value : firstName),
                        new SqlParameter("@LastName",string.IsNullOrEmpty(lastName) ? DBNull.Value : lastName),
                        new SqlParameter("@SignUpFromDate",signUpFromDate.HasValue ? (object)signUpFromDate.Value : DBNull.Value),
                        new SqlParameter("@SignUpToDate",signUpToDate.HasValue ? (object)signUpToDate.Value : DBNull.Value),
                       new SqlParameter("@SortColumn", sortColumn),
                       new SqlParameter("@SortDirection", sortDirection)
         };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);

                    // var result = await sqlHelpers.ExecuteReaderAsync(storedProcName, parameters);

                    using (SqlDataReader dr = await sqlHelpers.ExecuteReaderAsync(storedProcName, parameters))
                    {
                        while (dr.Read())
                        {

                            var findShopper = new FindShoppers
                            {
                                UserDetailId = Convert.ToInt32(dr["UserDetailId"]),
                                Email = dr["Email"].ToString() ?? string.Empty,
                                BarCodeValue = dr["BarCodeValue"].ToString() ?? string.Empty,
                                Mobile = dr["Mobile"].ToString() ?? string.Empty,
                                ZipCode = dr["ZipCode"].ToString() ?? string.Empty,
                                ClientStoreId = dr["ClientStoreId"] != DBNull.Value ? Convert.ToInt32(dr["ClientStoreId"]) : (int?)null,
                                FirstName = dr["FirstName"].ToString() ?? string.Empty,
                                LastName = dr["LastName"].ToString() ?? string.Empty,
                                SignUpDate = dr["SignUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["SignUpDate"]) : (DateTime?)null,
                            };

                            findShopperPagination.FindShopperList.Add(findShopper);
                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                findShopperPagination.TotalCount = Convert.ToInt32(dr["TotalRecords"]);
                                findShopperPagination.TotalPages = Convert.ToInt32(dr["TotalPages"]);
                            }
                        }
                    }


                    return findShopperPagination;




                }

            }
            return null;
        }


        public async Task<FindShopperPagination> GetFilteredUserDetails(string clientName, int? page, int? limit, string? sortColumn, string? sortDirection, string? searchTerm)
        {
            FindShopperPagination findShopperPagination = new FindShopperPagination();
            findShopperPagination.FindShopperList = new List<FindShoppers>();

            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    //var storedProcName = "pagination_data";
                    var storedProcName = "GetPagedSortedFilteredUserDetails";
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    SqlParameter[] parameters = new SqlParameter[]
         {
                     new SqlParameter("@PageNumber", !page.HasValue ? 1 : page),
                    new SqlParameter("@PageSize", !limit.HasValue ? 5 : 0),


                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@SearchTerm",string.IsNullOrEmpty(searchTerm ) ? string.Empty : searchTerm ),

         };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);

                    // var result = await sqlHelpers.ExecuteReaderAsync(storedProcName, parameters);

                    using (SqlDataReader dr = await sqlHelpers.ExecuteReaderAsync(storedProcName, parameters))
                    {
                        while (dr.Read())
                        {

                            var findShopper = new FindShoppers
                            {
                                UserDetailId = Convert.ToInt32(dr["UserDetailId"]),
                                Email = dr["Email"].ToString() ?? string.Empty,
                                BarCodeValue = dr["BarCodeValue"].ToString() ?? string.Empty,
                                Mobile = dr["Mobile"].ToString() ?? string.Empty,
                                ZipCode = dr["ZipCode"].ToString() ?? string.Empty,
                                ClientStoreId = dr["ClientStoreId"] != DBNull.Value ? Convert.ToInt32(dr["ClientStoreId"]) : (int?)null,
                                FirstName = dr["FirstName"].ToString() ?? string.Empty,
                                LastName = dr["LastName"].ToString() ?? string.Empty,
                                SignUpDate = dr["SignUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["SignUpDate"]) : (DateTime?)null,
                            };

                            findShopperPagination.FindShopperList.Add(findShopper);
                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                findShopperPagination.TotalCount = Convert.ToInt32(dr["TotalRecords"]);
                                //findShopperPagination.Page = Convert.ToInt32(dr["TotalPages"]);
                            }
                        }
                    }


                    return findShopperPagination;




                }

            }
            return null;
        }

        

       // public async Task<List<CreateShopperGroupModel>> CreateShopperGroups(string clientName, CreateShopperGroupModel model)
        public async Task<List<CreateShopperGroupModel>> CreateShopperGroups(string clientName, string? SIGNFROMDATE, string? SIGNUPTODATE, string? FIRSTNAME, string? LASTNAME,
            string? USERNAME, string? ZIPCODE, int? STOREID, string? MEMBERNUMBER, string? GroupName, string? Description, int UserDetailId)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "Create_Shopper_Groups";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@SIGNFROMDATE",string.IsNullOrEmpty(SIGNFROMDATE) ? string.Empty : SIGNFROMDATE),
                         new SqlParameter("@SIGNUPTODATE",string.IsNullOrEmpty(SIGNUPTODATE) ? string.Empty : SIGNUPTODATE),
                         new SqlParameter("@FIRSTNAME",string.IsNullOrEmpty(FIRSTNAME) ? string.Empty : FIRSTNAME ),
                          new SqlParameter("@LASTNAME",string.IsNullOrEmpty(LASTNAME) ? string.Empty : LASTNAME),
                           new SqlParameter("@USERNAME",string.IsNullOrEmpty(USERNAME) ? string.Empty : USERNAME),
                           new SqlParameter("@ZIPCODE",string.IsNullOrEmpty(ZIPCODE) ? string.Empty : ZIPCODE),
                           new SqlParameter("@STOREID",STOREID),
                          new SqlParameter("@MEMBERNUMBER",string.IsNullOrEmpty(MEMBERNUMBER) ? string.Empty : MEMBERNUMBER),
                          new SqlParameter("@GroupName",string.IsNullOrEmpty(GroupName) ? string.Empty : GroupName),
                          new SqlParameter("@Description",string.IsNullOrEmpty(Description) ? string.Empty : Description),
                          new SqlParameter("@UserDetailId",UserDetailId)

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsAffected = await sqlHelpers.InsertTable(storedProcName, parameters);

                    if (rowsAffected > 0)
                    {
                        var data = new List<CreateShopperGroupModel>()
                        {

                            new CreateShopperGroupModel()
                            {
                                SIGNFROMDATE = SIGNFROMDATE,
                                SIGNUPTODATE = SIGNUPTODATE,
                                FIRSTNAME = FIRSTNAME,
                                LASTNAME = LASTNAME,
                                USERNAME = USERNAME,
                                ZIPCODE = ZIPCODE,
                                STOREID =  STOREID ?? 0,
                             MEMBERNUMBER = MEMBERNUMBER,
                             GroupName = GroupName,
                             Description = Description
                            }
                        };
                     
                        return data;
                    }





                }

            }
            return null;
        }

        public async Task<List<UserGroups>> GetUserGroups(string clientName, int userId)
        {
           List<UserGroups> userGroups = null;
            userGroups = new List<UserGroups>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GetUserGroupsByUserId";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@UserId",userId),
                        

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                    
                         userGroups = ( from dr in result 
                                        select (
                                        new UserGroups
                                        {
                                          ClubId = Convert.ToInt32(dr["ClubId"]),
                                          Name = dr["Name"].ToString() ?? string.Empty,
                                          clubdetails = dr["clubdetails"].ToString() ?? string.Empty,
                                          IsMemberIDRequired = Convert.ToBoolean(dr["IsMemberIDRequired"]),
                                          IsEnableOnSignOn = Convert.ToBoolean(dr["IsEnableOnSignOn"]),
                                          createddate = Convert.ToDateTime(dr["createddate"]).ToString("MM/dd/yyyy"),
                                          ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]).ToString("MM/dd/yyyy"),

                                        })
                                        ).ToList();
                        return userGroups;
                    }





                }

            }

            return null;
        }

        public  async Task<List<UserGroups>> GetAvailableUserGroups(string clientName, int userId)
        {
            List<UserGroups> userGroups = null;
            userGroups = new List<UserGroups>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GetAvailableGroups";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@UserId",userId),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {

                        userGroups = (from dr in result
                                      select (
                                      new UserGroups
                                      {
                                          ClubId = Convert.ToInt32(dr["ClubId"]),
                                          Name = dr["Name"].ToString() ?? string.Empty,
                                          clubdetails = dr["clubdetails"].ToString() ?? string.Empty,
                                          IsMemberIDRequired = Convert.ToBoolean(dr["IsMemberIDRequired"]),
                                          IsEnableOnSignOn = Convert.ToBoolean(dr["IsEnableOnSignOn"]),
                                          createddate = Convert.ToDateTime(dr["createddate"]).ToString("MM/dd/yyyy"),
                                          ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]).ToString("MM/dd/yyyy"),

                                      })
                                       ).ToList();
                        return userGroups;
                    }





                }

            }

            return null;
        }

        public async Task<int> AddGroups(string clientName, int userId, int clubId)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "AddAvailableGroups";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@UserId",userId),
                         new SqlParameter("@ClubId",clubId),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.InsertTable(storedProcName, parameters);

                    if (rowsEffected > 0)
                    {

                       
                        return 1;
                    }





                }

            }

            return 0;

        }

        public async Task<int> DeleteUserGroups(string clientName, int userId, int clubId)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "DeleteGroupUserByUserIdandClubId";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@UserId",userId),
                         new SqlParameter("@ClubId",clubId),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.UpdateTable(storedProcName, parameters);

                    if (rowsEffected > 0)
                    {


                        return 1;
                    }





                }

            }

            return 0;
        }
        //public async Task<int> ValidateExistingRole(string clientName, string applicationName, string roleName)
        public async Task<int> ValidateExistingRole(string clientName,  string roleName)

        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };


                    

                    //var storedProcName = "aspnet_Roles_RoleExists";
                    var storedProcName = "ValidateExistingRole";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                    var parameters = new SqlParameter[]
                    {
                       // new SqlParameter("@ApplicationName",applicationName ?? string.Empty),
                        new SqlParameter("@RoleName",roleName ?? string.Empty),


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.ExecuteIntScalar(storedProcName, parameters);

                    if (rowsEffected > 0)
                    {


                        return 1;
                    }





                }

            }


            return 0;
        }
        public async Task<int> CreateMembershipUser(string clientName,AspnetMembershipCreateUser aspnetMembershipCreateUser)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };


             

                    var storedProcName = "dbo.aspnet_Membership_CreateUser";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;   
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@ApplicationName","/"),
                        new SqlParameter("@UserName",aspnetMembershipCreateUser.UserName),
                        new SqlParameter("@Password",aspnetMembershipCreateUser.Password),
                        new SqlParameter("@PasswordSalt",aspnetMembershipCreateUser.PasswordSalt),
                        new SqlParameter("@Email",aspnetMembershipCreateUser.Email),
                        new SqlParameter("@PasswordQuestion",aspnetMembershipCreateUser.PasswordQuestion),
                        new SqlParameter("@PasswordAnswer",aspnetMembershipCreateUser.PasswordAnswer),
                        new SqlParameter("@IsApproved",aspnetMembershipCreateUser.IsAproved),
                        new SqlParameter("@CurrentTimeUtc",aspnetMembershipCreateUser.CurrentTimeUtc),
                        new SqlParameter("@CreateDate",aspnetMembershipCreateUser.CreateDate),
                        new SqlParameter("@UniqueEmail",aspnetMembershipCreateUser.UniqueEmail),
                        new SqlParameter("@PasswordFormat",aspnetMembershipCreateUser.PasswordFormat),
                        new SqlParameter("@UserId",aspnetMembershipCreateUser.UserId.ToString().ToUpper())

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.InsertTable(storedProcName, parameters);

                    if (rowsEffected > 0)
                    {


                        return 1;
                    }





                }

            }

            return 0;
        }

        public async Task<Get_aspnet_userId_result> GetUserIdbyUserName(string clientName, string userName)
        {
            Get_aspnet_userId_result aspnet_UserId_Result = new Get_aspnet_userId_result();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GetAspnetUserID";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                    var parameters = new SqlParameter[]
                    {
                        
                        new SqlParameter("@UserName",userName),
                        

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetSingleRow(storedProcName, parameters);

                    if (result != null) {
                        aspnet_UserId_Result.UserId = (Guid)result["UserId"];
                        return aspnet_UserId_Result;
                   
                    }





                }

            }

            return null;

        }

        public async Task<BarCodeResult> GetBarcode(int customerId)
        {
            BarCodeResult barCodeResult = null;
            barCodeResult = new BarCodeResult();
             
            var appdata = new 
            {
                IsBarcodeType39 = "1",
                NCRMemberNumberPrefixValue = "44",
                MinimumMemberNumber = "100000000",
               MaximumMemberNumber = "999999999",
               CheckDuplicateMemberNumber = "0",
                AddCheckDigitToMemberNumber = "0",
                ByteScoutRegistrationEmail ="",
               ByteScoutRegistrationKey  ="",
            };
            Barcode barcode = new Barcode();
            Random random = new Random();
            barcode.Symbology = Convert.ToBoolean(Convert.ToInt32(appdata?.IsBarcodeType39?.ToString())) ? SymbologyType.Code39 : SymbologyType.Code128;
            barcode.Value =string.Concat(appdata?.NCRMemberNumberPrefixValue.ToString(),random.Next(Convert.ToInt32(appdata?.MinimumMemberNumber), Convert.ToInt32(appdata?.MaximumMemberNumber)).ToString());

            barCodeResult.BarCodeImage = string.Concat(customerId, "_", DateTime.Now.ToString("MMddyyyy_HHmmssfff"), ".jpg");
            if (Convert.ToBoolean(Convert.ToInt32(appdata?.CheckDuplicateMemberNumber)))
            {

            }
            if (Convert.ToBoolean(Convert.ToInt32(appdata?.AddCheckDigitToMemberNumber)))
            {
                barcode.Value = string.Concat(barcode.Value, random.Next(0, 9));
            }
            barCodeResult.BarCodeValue = barcode.Value;
            barcode.RegistrationKey = appdata?.ByteScoutRegistrationKey;
            barcode.RegistrationName = appdata?.ByteScoutRegistrationEmail;
            barcode.Angle = RotationAngle.Degrees0;
            barcode.Margins.Top = 5;


            return await Task.FromResult(barCodeResult);
        }

        public async Task<int> CreateRole(string clientName, string applicationName, string roleName)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "aspnet_Roles_CreateRole";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@ApplicationName",applicationName),
                        new SqlParameter("@RoleName",roleName),
                       

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.InsertTable(storedProcName, parameters);

                    if (rowsEffected > 0)
                    {


                        return 1;
                    }





                }

            }

            return 0;
        }

        public async Task<int> ValidateUser(string clientName, string userName)
        {
            

            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "ValidateUserName";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserName",userName),
                       


                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.ExecuteIntScalar(storedProcName, parameters);

                    if (rowsEffected > 0)
                    {


                        return 1;
                    }





                }

            }


            return 0;
        }
        public async Task<int> SaveShopperDetails(string clientName, UserDetailstoSave model)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };



                    //SaveUserDetail
                    //var storedProcName = "SaveShopperDetailsData";
                    var storedProcName = "SaveUserDetail";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UserDetailId",model.UserDetailId),
                        new SqlParameter("@UserName",model.UserName),
                        new SqlParameter("@Email",model.Email),
                        new SqlParameter("@Mobile",model.Mobile),
                        //new SqlParameter("@UserGUID",model.CustomerId),
                        new SqlParameter("@CustomerId",model.CustomerId),
                        new SqlParameter("@CustomerCode",model.CustomerCode),
                         new SqlParameter("@FirstName",model.FirstName),
                        new SqlParameter("@LastName",model.LastName),
                         new SqlParameter("@DeviceId",model.DeviceId),
                        new SqlParameter("@DeviceType",model.DeviceType),
                         new SqlParameter("@UserStatusId",model.UserStatusId),
                         new SqlParameter("@IsDeleted",model.IsDeleted),
                          new SqlParameter("@BarCodeImage",model.BarCodeImage),
                        new SqlParameter("@BarCodeValue",model.BarCodeValue),

                        new SqlParameter("@QRCodeImage",""),
                         new SqlParameter("@QRCodeValue",""),

                       // new SqlParameter("@IsActive",model.IsActive),
                       // new SqlParameter("@CreatedDate",model.CreatedDate),
                      //  new SqlParameter("@ModifiedDate",model.ModifiedDate),


                         new SqlParameter("@CompanyCustomerFK",model.CompanyCustomerFK),


                        new SqlParameter("@UserTypeId",model.UserTypeId),
                        new SqlParameter("@ZipCode",model.ZipCode),
                        new SqlParameter("@ClientStoreId",model.ClientStoreId),
                        new SqlParameter("@QToken",model.QToken)



,

                    };
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int rowsEffected = await sqlHelpers.InsertTable(storedProcName, parameters);

                    if (rowsEffected > 0 || rowsEffected == -1)
                    {


                        return 1;
                    }





                }

            }


            return 0;
        }

        public async Task<ResponseShopper> CreateShopper(string clientName, CreateShopperModel model)
        {
            ResponseShopper responseShopper = null;
            responseShopper = new ResponseShopper();
            var validatedUser = await ValidateUser(clientName, model.UserName);

            if(validatedUser == 0)
            {
              if(model.Password != model.ConfirmPassword)
                {
                    responseShopper.ErrorCode = "400";
                    responseShopper.ErrorMessage = "Passwords does not matched.";
                    responseShopper.Status = "Failed";
                   return responseShopper;

                }
                var encryptPass = await _helpers.EncryptAes(model.Password);
                var salt = await _helpers.GenerateSalt();
                var passwordSalt = Convert.ToBase64String(salt);
                var hashPassword = await _helpers.HashPasswordWithSaltAsync(model.Password, salt);
                AspnetMembershipCreateUser newUser = new AspnetMembershipCreateUser()
                {
                    ApplicationName = "/",
                    UserName = model.UserName,
                    //Password = model.Password,
                    //PasswordSalt = model.ConfirmPassword,
                    Password = encryptPass,
                    PasswordSalt = passwordSalt,
                    Email = model.UserName,
                    PasswordAnswer = "1",
                    PasswordQuestion = "1",
                    IsAproved = true,
                    CurrentTimeUtc = DateTime.Now,
                    CreateDate = DateTime.Now,
                    UniqueEmail = 1,
                    PasswordFormat = 1,
                    UserId = Guid.NewGuid(),
                };
                var createUser = await CreateMembershipUser(clientName, newUser);


                var roleExist = await ValidateExistingRole(clientName, model.RoleName);
                if (roleExist < 0)
                {
                    var createRole = await CreateRole(clientName, "/", "PromotionManager");
                }
                int customerID = 3;
                var barcode = await GetBarcode(customerID);

                Console.WriteLine("barcode", barcode);

                var providerKey = await GetUserIdbyUserName(clientName, model.UserName);

                Guid userGuid = new Guid(providerKey?.UserId.ToString());

                string qtoken = string.Empty;

                // save create shopper details



                var saveShopperDetails = new UserDetailstoSave()
                {
                    CustomerCode = Guid.NewGuid().ToString().Replace("_", string.Empty),
                    CustomerId = userGuid,
                    Email = model.UserName,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Mobile = model.Mobile,
                    UserName = model.UserName,
                    UserDetailId = 0,
                    UserId = 0,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DeviceId = 0,
                    DeviceType = "",
                    BarCodeImage = barcode.BarCodeImage,
                    BarCodeValue = barcode.BarCodeValue,
                    UserTypeId = model.UserTypeId,
                    UserStatusId = 1,
                    CompanyCustomerFK = 3,
                    ClientStoreId = 1,
                    ZipCode = model.ZipCode,
                    QToken = string.IsNullOrEmpty(qtoken) ? Guid.NewGuid().ToString() : string.Empty,
                    ExistingMemberNumber = string.Empty


                };

                var result = await SaveShopperDetails(clientName, saveShopperDetails);

                if (result > 0)
                {
                    responseShopper.ErrorMessage = "Successfully saved shopper details";
                    responseShopper.ErrorCode = "200";
                    responseShopper.Status = "Success";
                    return responseShopper;

                }
                else
                {
                    responseShopper.ErrorMessage = "Failed to save shopper details";
                    responseShopper.ErrorCode = "400";
                    responseShopper.Status = "Failed";
                    return responseShopper;
                }

            }
            else
            {
                responseShopper.ErrorMessage = "User Name already existed";
                responseShopper.ErrorCode = "400";
                responseShopper.Status = "Failed";
                return responseShopper;
            }

           

           return null;
        }

        public async Task<List<UserTypes>> GetUserTypes(string clientName)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GetUserTypes";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);
                 
                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                  var result   = await sqlHelpers.GetMultipleRows(storedProcName, null);

                   List<UserTypes> userTypes = new List<UserTypes>();

                    if (result.Count > 0) { 
                      
                        userTypes = (from dr in result 
                                     select(
                                     new UserTypes
                                     {
                                        Id = Convert.ToInt32(dr["Id"]), 
                                        UserType = dr["UserType"].ToString() ?? string.Empty
                                     }
                                     )).ToList();
                    
                      return userTypes;
                    }




                }

            }


            return null;
        }

        public async Task<List<GetRoles>> GetAllRoles(string clientName)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GetRoles";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    // get presenttime 
                    DateTime time = DateTime.Now;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(1);

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, null);

                    List<GetRoles> roles = new List<GetRoles>();

                    if (result.Count > 0)
                    {

                        roles = (from dr in result
                                     select (
                                     new GetRoles
                                     {
                                        // RoleId = (Guid)dr["RoleId"],
                                        RoleId = dr["RoleId"]?.ToString()?.ToUpper() ?? string.Empty,
                                         RoleName = dr["RoleName"].ToString() ?? string.Empty
                                     }
                                     )).ToList();

                        return roles;
                    }




                }

            }


            return null;
        }

        public async Task<List<AllShoppersGroups>> GetAllShoppersGroups(string clientName,int groupId,int userId)
        {
             List<AllShoppersGroups> allShoppers = new List<AllShoppersGroups>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };
                     //var data = await GetGroupAnalysisTimeLine(clientName, groupId, userId);    
                   
                  // var  storedProc = "PROC_CUSTOM_GET_GROUP_ANALYSIS_TIMELINE" ;
                     

                    var storedProcName = "PROC_CUSTOM_GET_ALL_SHOPPER_GROUPS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GroupID",groupId),
                        new SqlParameter("@UserID",userId)
                    };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);

                  
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                  

                    if (result.Count > 0)
                    {

                        allShoppers = (from dr in result
                                 select (
                                 new AllShoppersGroups
                                 {
                                     GroupName = dr["GroupName"].ToString() ?? string.Empty,
                                     GroupDetails = dr["GroupDetails"].ToString() ?? string.Empty,
                                     GroupID = Convert.ToInt32(dr["GroupID"]),
                                     CreatedOn = Convert.ToDateTime(dr["CreatedOn"]).ToString("MM/dd/yyyy"),
                                     TotalShoppers = Convert.ToInt32(dr["TotalShoppers"]),
                                     TopicARN = dr["TopicARN"].ToString() ?? string.Empty
                                 }
                                 )).ToList();

                        return allShoppers;
                    }




                }

            }

            return null;
        }

        public async Task<List<ProductsModel>> GetAllTimeProducts(string clientName, int GroupId)
        {
            List<ProductsModel> products = new List < ProductsModel > ();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_ALLTIME_PRODUTS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GrouPId",GroupId),
                        
                    };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                      products = (from dr in result
                                       select (
                                       new ProductsModel
                                       {
                                           ProductName = dr["ProductName"].ToString() ?? string.Empty,
                                           UPC = dr["UPC"].ToString() ?? string.Empty,
                                          AMOUNT = Convert.ToDecimal(dr["AMOUNT"]),
                                          
                                       }
                                       )).ToList();

                        return products;
                    }




                }

            }

            return null;
        }

        public async Task<List<ProductsModel>> GetTopProducts(string clientName, int GroupId, int NoOfDays)
        {
           // var data = await DownloadUserShopperReport(clientName, 99);

            List<ProductsModel> products = new List<ProductsModel>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_GROUP_TOP_PRODUTS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GROUPID",GroupId),
                        new SqlParameter("@NOOFDAYS",NoOfDays),

                    };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);



                    if (result.Count > 0)
                    {

                        products = (from dr in result
                                    select (
                                    new ProductsModel
                                    {
                                        ProductName = dr["ProductName"].ToString() ?? string.Empty,
                                        UPC = dr["UPC"].ToString() ?? string.Empty,
                                        AMOUNT = Convert.ToDecimal(dr["AMOUNT"]),

                                    }
                                    )).ToList();

                        return products;
                    }




                }

            }

            return null;
        }

        public async Task<GroupAnalysysTimeLineResponse> GetGroupAnalysisTimeLine(string clinetName, int groupId, int userId)
        {
            GroupAnalysysTimeLineResponse response = null;
            response = new GroupAnalysysTimeLineResponse();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clinetName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_GROUP_ANALYSIS_TIMELINE";
                    //var storedProcName = "search_records_without_limit";
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GroupID",groupId),
                        new SqlParameter("@UserID",userId)
                    };


                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);

                   

                    using (SqlDataReader dr = await sqlHelpers.ExecuteReaderAsync(storedProcName, parameters))
                    {
                        while (dr.Read())
                        {
                            // club
                            var groupDetails = new Club
                            {
                                ClubId = dr["ClubId"] != DBNull.Value ? Convert.ToInt32(dr["ClubId"]) : 0,
                                Name = dr["Name"]?.ToString() ?? string.Empty,
                                ClubDetails = dr["ClubDetails"]?.ToString() ?? string.Empty,
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).ToString("MM/dd/yyyy") : string.Empty
                            };
                            response.GroupDetails = groupDetails;
                        }

                        // table for startdetails
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var startDetails = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                                response.GroupStartDetails = startDetails;
                            }
                        }
                        // summery details
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var summeryDetails = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                               response.GroupSummeryDetails = summeryDetails;
                            }
                        }
                        // fromdays 1 
                        
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var fromdays1details = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                               response.Fromdays1 = fromdays1details;
                            }
                        }
                        // fromday2
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var fromdays2details = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                                response.Fromdays2 = fromdays2details;
                            }
                        }

                        // fromday3
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var fromdays3details = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                                response.Fromdays3 = fromdays3details;
                            }
                        }
                        // fromday4
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var fromdays4details = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                                response.Fromdays4 = fromdays4details;
                            }
                        }

                        // fromday5
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var fromdays5details = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                                response.Fromdays5 = fromdays5details;
                            }
                        }
                        // fromday6
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var fromdays6details = new GroupsModels
                                {
                                    TOTALSHOPPERS = dr["TOTALSHOPPERS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALSHOPPERS"]) : 0,
                                    TOTALTRANSACTIONS = dr["TOTALTRANSACTIONS"] != DBNull.Value ? Convert.ToInt32(dr["TOTALTRANSACTIONS"]) : 0,
                                    TOTALCOUPONVALUE = dr["TOTALCOUPONVALUE"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALCOUPONVALUE"]) : 0,
                                    TOTALINCOME = dr["TOTALINCOME"] != DBNull.Value ? Convert.ToDecimal(dr["TOTALINCOME"]) : 0,
                                    AVGTRANSACTION = dr["AVGTRANSACTION"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTRANSACTION"]) : 0,
                                    GROUPSTATUS = dr["GROUPSTATUS"]?.ToString() ?? string.Empty,
                                    FROMDATE = dr["FROMDATE"] != DBNull.Value ? Convert.ToDateTime(dr["FROMDATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                    TODATE = dr["TODATE"] != DBNull.Value ? Convert.ToDateTime(dr["TODATE"]).ToString("MM/dd/yyyy") : string.Empty,
                                };
                                response.Fromdays6 = fromdays6details;
                            }
                        }
                    }
                    return response;




                }

            }

            return response;
        }

        public async Task<List<DownloadShoppers>> DownloadUserShopperReport( string clientName,int groupId)
        {
            List<DownloadShoppers> downloadShoppers = new List<DownloadShoppers>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_DOWNLOAD_SHOPPERS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GroupId",groupId),
                       

                    };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    //var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    DataTable result = await sqlHelpers.ReadDataTableAsync(storedProcName, parameters);

                   // DataTable result
                   if (result.Rows.Count > 0)     // if(result.count > 0) // datarows
                    {

                        // data rows

                        //downloadShoppers = (from dr in result
                        //            select (
                        //            new DownloadShoppers
                        //            {
                        //                UserName = dr["UserName"].ToString() ?? string.Empty,
                        //                 BarcodeValue= dr["BarcodeValue"].ToString() ?? string.Empty,
                        //                StoreName = dr["StoreName"].ToString() ?? string.Empty,

                        //            }
                        //            )).ToList();

                        //return downloadShoppers;

                      var  downloadShopper = result.AsEnumerable().Select(s => new DownloadShoppers
                        {
                            UserName = s.Field<string>("UserName") ?? string.Empty,
                            BarcodeValue = s.Field<string>("BarcodeValue") ?? string.Empty,
                            StoreName = s.Field<string> ("StoreName") ?? string.Empty
                        }).ToList();    

                        return downloadShopper;
                    }




                }

            }

            return null;
        }

        public async Task<int> PreDefinedShopperGroupsByLastShoppedDate(string clientName, PreDefinedShopperGroupsByLastShopped model)
        {


            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_LAST_SHOPPED_DATE";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NoOfDaysSinceShopped",model?.NoOfDaysSinceShopped),
                        new SqlParameter("@GroupName",model?.GroupName)
                    };
                   

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                   int result = await sqlHelpers.InsertTable(storedProcName, parameters);

                  

                    if(result > 0 || result == -1)
                    {
                        return 1;
                    }




                }

            }


            return 0;
        }

        public async Task<int> PreDefinedShopperGroupByZipcodesList(string clientName, PreDefinedShopperGroupByZipcodes model)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_ZIPCODE";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@ZipcodeList",model?.ZipcodeList),
                        new SqlParameter("@AllUsers",model?.AllUsers),
                        new SqlParameter("@SinceRegistered",model?.SinceRegistered ?? (object)DBNull.Value),
                    };


                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int result = await sqlHelpers.InsertTable(storedProcName, parameters);



                    if (result > 0 || result == -1)
                    {
                        return 1;
                    }




                }

            }


            return 0;
        }

        public async Task<int> PreDefinedShopperGroupsByUPCList(string clientName, PreDefinedShopperGroupsByUPCLists model)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_CREATE_SHOPPER_GROUP_BY_UPC_LIST";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GroupName",model?.GroupName),
                        new SqlParameter("@UPCList",model?.UPCList),
                        new SqlParameter("@NoOfTimesPurchased",model?.NoOfTimesPurchased ?? 0),
                        new SqlParameter("@NoOfDaysSinceShopped",model?.NoOfDaysSinceShopped ?? 0)
                    };


                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    int result = await sqlHelpers.InsertTable(storedProcName, parameters);



                    if (result > 0 || result == -1)
                    {
                        return 1;
                    }




                }

            }


            return 0;
        }

        public async Task<List<ProductCategories>> GetProductCategories(string clientName)
        {
            List<ProductCategories> products = new List<ProductCategories>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "CUSTOM_PROC_GET_PRODUCT_CATEGORIES";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                   


                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, null);

                    if (result.Count > 0) { 
                    
                        products = (from dr in result 
                                    select( new ProductCategories { 
                                        ProductCategoryId = dr["ProductCategoryId"] !=DBNull.Value ? Convert.ToInt32(dr["ProductCategoryId"]) :0,
                                        ProductCategoryName = dr["ProductCategoryName"].ToString() ?? string.Empty,
                                        ClientDepartmentID = dr["ClientDepartmentID"]!= DBNull.Value ? Convert.ToInt32(dr["ClientDepartmentID"]) : 0,
                                        DefaultProductID = dr["DefaultProductID"] !=DBNull.Value ? Convert.ToInt32(dr["DefaultProductID"]) :0,
                                        MajorDepartmentID = dr["MajorDepartmentID"] !=DBNull.Value ? Convert.ToInt32(dr["MajorDepartmentID"]) : 0
                                       

                                    })).ToList();
                        return products;
                    }





                }

            }


            return null;
        }

        public async Task<List<TopShoppers>> GetTopShoppers(string clientName,int noOfRecords,int storeId,string orderByDirection,DateTime? fromDate,DateTime? toDate,int? departmentId)
        {
            // var advance = await GetAdvancedSearchShoppers(clientName, "", null, null, 0, 1, "0", 0, 1, "0", 0, 1, "0", false);

            // var d = await GetFindShopperByUPCs("Veritra RSA","15", 0,fromDate,toDate );
            //var res = await GetProductsDetails(clientName, "", "", 0, false);

            // var d = await GetSearchAndCountDetails("Veritra RSA", 30, 360, "SAI TEST");
            // var d = await GetSearchAndCreateGroup("Veritra RSA",0,100, "Veritra Employees Groups");
            //   var d = await UploadShoppersToGroupsWithFile("Veritra RSA", "Sai test", "44279468347");
            //DateTime date = DateTime.Parse("2020-05-17");
            //DateTime da = DateTime.Parse("2020-06-22");
            //var d = await FindCoupons("Veritra RSA", 4, date, da);
            //var da = await GetNewsCategories("Veritra RSA");
            List<TopShoppers> topShoppers = new List<TopShoppers>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_TOP_CUSTOMERS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NOOFRECORDS",noOfRecords),
                        new SqlParameter("@STOREID",storeId),
                        new SqlParameter("@ORDERBYDIRECTION",orderByDirection),
                        new SqlParameter("@BEGINDATE",fromDate ?? null),
                        new SqlParameter("@ENDDATE",toDate ?? null),
                        new SqlParameter("@DEPARTMENTID",departmentId != 0 ? departmentId : 0),
                       
                    };



                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                       topShoppers = (from dr in result
                                           select new TopShoppers
                                           {
                                               LOYALTYID = dr["LOYALTYID"] != DBNull.Value ? Convert.ToInt64(dr["LOYALTYID"]) : 0,
                                               FIRSTNAME = dr["FIRSTNAME"].ToString() ?? string.Empty,
                                               LASTNAME = dr["LASTNAME"].ToString() ?? string.Empty,
                                               USERNAME = dr["USERNAME"].ToString() ?? string.Empty,
                                               USERDETAILID = dr["USERDETAILID"] != DBNull.Value ? Convert.ToInt64(dr["USERDETAILID"]) : 0,
                                               STOREID = dr["STOREID"] != DBNull.Value ? Convert.ToInt64(dr["STOREID"]) : 0,
                                               STORENAME = dr["STORENAME"].ToString() ?? string.Empty,
                                               VISITSCOUNT = dr["VISITSCOUNT"] != DBNull.Value ? Convert.ToInt64(dr["VISITSCOUNT"]) : 0,
                                               TOTAL = dr["TOTAL"] != DBNull.Value ? Convert.ToDecimal(dr["TOTAL"]) : 0.0m,
                                               AVGBASKET = dr["AVGBASKET"] != DBNull.Value ? Convert.ToDecimal(dr["AVGBASKET"]) : 0.0m,
                                               TOTALBASKETAMOUNT = dr["TOTALBASKETAMOUNT"] != DBNull.Value ? Convert.ToDecimal(Convert.ToDecimal(dr["TOTALBASKETAMOUNT"]).ToString("F2")) : 0.00m,
                                           }).ToList();
                        return topShoppers;
                    }





                }

            }


            return null;
        }

        public async Task<List<AdvancedSearchShopper>>  GetShoppersAdvancedSearch(string memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string clubId, int? minBasketCount, int? maxBasketCount, string storeId, int? minReedemCount, int? maxRedeemCount, string departmentId, bool? isCreatedGroup)
        {
            List<AdvancedSearchShopper> shoppers = new List<AdvancedSearchShopper>();
             string connectionString = "Data Source=veritrarsadevdb.clkbmvhusrdv.us-east-1.rds.amazonaws.com;Initial Catalog=RSA_GroceryDEV;User ID=Veritragrocerydblogin;Password=otTL5kd0buqb7erEuTqX";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[PROC_CUSTOM_GET_SHOPPERS_ADVANCED_SEARCH]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

int parsedMinSpend = minSpend.HasValue ? minSpend.Value : 0;
int parsedMaxSpend = maxSpend.HasValue ? maxSpend.Value : 0;
int parsedMinBasketCount = minBasketCount.HasValue ? minBasketCount.Value : 0;
int parsedMaxBasketCount = maxBasketCount.HasValue ? maxBasketCount.Value : 0;
int parsedMinRedeemCount = minReedemCount.HasValue ? minReedemCount.Value : 0;
int parsedMaxRedeemCount = maxRedeemCount.HasValue ? maxRedeemCount.Value : 0;

// Add parameters to the command, with empty string handling for non-numeric fields
command.Parameters.AddWithValue("@MemberNumber", string.IsNullOrWhiteSpace(memberNumber) ? "": memberNumber);
command.Parameters.AddWithValue("@TransactionFrom", transactionFrom ?? (object)DBNull.Value);
command.Parameters.AddWithValue("@TransactionTo", transactionTo ?? (object)DBNull.Value);

// Safe numeric values
command.Parameters.AddWithValue("@MinSpend", parsedMinSpend);
command.Parameters.AddWithValue("@MaxSpend", parsedMaxSpend);

// Ensure no empty strings for non-numeric fields
command.Parameters.AddWithValue("@Clubid", string.IsNullOrWhiteSpace(clubId) ? "0" : clubId);
command.Parameters.AddWithValue("@MinBasketCount", parsedMinBasketCount);
command.Parameters.AddWithValue("@MaxBasketCount", parsedMaxBasketCount);
command.Parameters.AddWithValue("@StoreId", string.IsNullOrWhiteSpace(storeId) ? "0" : storeId);

// Ensure boolean and other values are handled properly
command.Parameters.AddWithValue("@MinRedeemCount", parsedMinRedeemCount);
command.Parameters.AddWithValue("@MaxRedeemCount", parsedMaxRedeemCount);
command.Parameters.AddWithValue("@DepartmentId", string.IsNullOrWhiteSpace(departmentId) ? "0" : departmentId);
command.Parameters.AddWithValue("@IsCreateGroup", isCreatedGroup ?? false);
                    command.CommandTimeout = 120;
                    // Open the connection
                    connection.Open();

                    // Execute the command and retrieve data using SqlDataReader
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AdvancedSearchShopper shopper = new AdvancedSearchShopper
                            {
                                // loyaltyid = dr["loyaltyid"] != DBNull.Value ? Convert.ToInt64(dr["loyaltyid"]) : 0,
                                loyaltyid = dr["loyaltyid"].ToString() ?? string.Empty,
                                UserName = dr["UserName"].ToString() ?? string.Empty,
                                UserDetailId = dr["UserDetailId"] != DBNull.Value ? Convert.ToInt64(dr["UserDetailId"]) : 0,
                                AVGTrAmount = dr["AVGTrAmount"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTrAmount"]) : 0.0m,
                                BasketDataCount = dr["BasketDataCount"] != DBNull.Value ? Convert.ToInt32(dr["BasketDataCount"]) : 0,
                                FirstName = dr["FirstName"].ToString() ?? string.Empty,
                                LastName = dr["LastName"].ToString() ?? string.Empty,
                                TotalRecords = dr["TotalRecords"] != DBNull.Value ? Convert.ToInt32(dr["TotalRecords"]) : 0,
                                PreferredStore = dr["PreferredStore"].ToString() ?? string.Empty,
                            };

                            shoppers.Add(shopper);
                        }
                    }
                }
            }

            return await Task.FromResult(shoppers);
        }
        public async Task<List<AdvancedSearchShopper>> GetAdvancedSearchShoppers(string clientName, string? memberNumber, DateTime? transactionFrom, DateTime? transactionTo, int? minSpend, int? maxSpend, string? clubId, int? minBasketCount, int? maxBasketCount, string? storeId, int? minReedemCount, int? maxRedeemCount, string? departmentId, bool? isCreatedGroup)
        {
            // string memberNumber, DateTime transactionFrom, DateTime transactionTo, int minSpend, int maxSpend, string clubId, int minBasketCount, int maxBasketCount, string storeId, int minRedeemCount, int maxRedeemCount, string departmentId, bool isCreateGroup
        //    var advance = await GetShoppersAdvancedSearch(memberNumber, transactionFrom, transactionTo, minSpend, maxSpend, clubId, minBasketCount, maxBasketCount, storeId, minReedemCount, maxRedeemCount, departmentId, isCreatedGroup);
            List<AdvancedSearchShopper> advancedSearchShoppers = new List<AdvancedSearchShopper>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_SHOPPERS_ADVANCED_SEARCH";

                 //   string connectionString = "Data Source=veritrarsadevdb.clkbmvhusrdv.us-east-1.rds.amazonaws.com;Initial Catalog=RSA_GroceryDEV;User ID=Veritragrocerydblogin;Password=otTL5kd0buqb7erEuTqX";
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);

                    //                   var parameters = new SqlParameter[]
                    //{
                    //   new SqlParameter("@MemberNumber", string.IsNullOrEmpty(memberNumber) ? "" : memberNumber),
                    //   new SqlParameter("@TransactionFrom", transactionFrom.HasValue ? transactionFrom.Value : DBNull.Value),
                    //   new SqlParameter("@TransactionTo", transactionTo.HasValue ? transactionTo.Value : DBNull.Value),
                    //   new SqlParameter("@MinSpend", minSpend.HasValue ? minSpend.Value : 0),
                    //   new SqlParameter("@MaxSpend", maxSpend.HasValue ? maxSpend.Value : 100),
                    //   new SqlParameter("@Clubid", string.IsNullOrEmpty(clubId) ? "" : clubId),
                    //   new SqlParameter("@MinBasketCount", minBasketCount.HasValue ? minBasketCount.Value : 0),
                    //   new SqlParameter("@MaxBasketCount", maxBasketCount.HasValue ? maxBasketCount.Value : 0),
                    //   new SqlParameter("@StoreId", string.IsNullOrEmpty(storeId) ? "0" : storeId),
                    //   new SqlParameter("@MinRedeemCount", minReedemCount.HasValue ? minReedemCount.Value : 0),
                    //   new SqlParameter("@MaxRedeemCount", maxRedeemCount.HasValue ? maxRedeemCount.Value : 1),
                    //   new SqlParameter("@DepartmentId", string.IsNullOrEmpty(departmentId) ? "0" : departmentId),
                    //   new SqlParameter("@IsCreateGroup", isCreatedGroup.HasValue ? isCreatedGroup.Value : false),
                    //};


                    //                   var parameters = new SqlParameter[]
                    //{
                    //   new SqlParameter("@MemberNumber", SqlDbType.VarChar) { Value = memberNumber ?? "0" },
                    //   new SqlParameter("@TransactionFrom", SqlDbType.DateTime) { Value = transactionFrom },
                    //   new SqlParameter("@TransactionTo", SqlDbType.DateTime) { Value = transactionTo },
                    //   new SqlParameter("@MinSpend", SqlDbType.Int) { Value = minSpend },
                    //   new SqlParameter("@MaxSpend", SqlDbType.Int) { Value = maxSpend },
                    //   new SqlParameter("@Clubid", SqlDbType.VarChar) { Value = clubId ?? "0" },
                    //   new SqlParameter("@MinBasketCount", SqlDbType.Int) { Value = minBasketCount },
                    //   new SqlParameter("@MaxBasketCount", SqlDbType.Int) { Value = maxBasketCount },
                    //   new SqlParameter("@StoreId", SqlDbType.VarChar) { Value = storeId ?? "0" },
                    //   new SqlParameter("@MinRedeemCount", SqlDbType.Int) { Value = minReedemCount },
                    //   new SqlParameter("@MaxRedeemCount", SqlDbType.Int) { Value = maxRedeemCount },
                    //   new SqlParameter("@DepartmentId", SqlDbType.VarChar) { Value = departmentId ?? "0" },
                    //   new SqlParameter("@IsCreateGroup", SqlDbType.Bit) { Value = isCreatedGroup }
                    //};

                    var parameters = new SqlParameter[]
                    {
                         new SqlParameter("@MemberNumber", "0"),
    new SqlParameter("@TransactionFrom", DateTime.Parse("2024-12-05")),
    new SqlParameter("@TransactionTo", DateTime.Parse("2024-12-12")),
    new SqlParameter("@MinSpend", 1),
    new SqlParameter("@MaxSpend", 100),
    new SqlParameter("@Clubid", "0"),
    new SqlParameter("@MinBasketCount", 1),
    new SqlParameter("@MaxBasketCount", 10),
    new SqlParameter("@StoreId", "0"),
    new SqlParameter("@MinRedeemCount", 1),
    new SqlParameter("@MaxRedeemCount", 10),
    new SqlParameter("@DepartmentId", "0"),
    new SqlParameter("@IsCreateGroup", false)
                    };













                    var parameter = new SqlParameter[]
                      {
        new SqlParameter("@MemberNumber", memberNumber ?? string.Empty),
        new SqlParameter("@TransactionFrom", transactionFrom),
        new SqlParameter("@TransactionTo", transactionTo),
        new SqlParameter("@MinSpend", minSpend),
        new SqlParameter("@MaxSpend", maxSpend),
        new SqlParameter("@Clubid", clubId ?? string.Empty),
        new SqlParameter("@MinBasketCount", minBasketCount),
        new SqlParameter("@MaxBasketCount", maxBasketCount),
        new SqlParameter("@StoreId", storeId ?? string.Empty),
        new SqlParameter("@MinRedeemCount", minReedemCount),
        new SqlParameter("@MaxRedeemCount", maxRedeemCount),
        new SqlParameter("@DepartmentId", departmentId ?? string.Empty),
        new SqlParameter("@IsCreateGroup", isCreatedGroup)
                      };

                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                  
                       var result = await sqlHelpers.GetMultipleRowsdata(storedProcName, parameters);
                    //var res = await sqlHelpers.GetMultipleDataRows(storedProcName, parameter);


                    if (result.Count > 0 && result != null)
                    {
                        advancedSearchShoppers = (from dr in result
                                       select new AdvancedSearchShopper
                                       {
                                           loyaltyid = dr["loyaltyid"] != DBNull.Value ? (dr["loyaltyid"].ToString()) : string.Empty,
                                           UserName = dr["UserName"].ToString() ?? string.Empty,
                                           UserDetailId = dr["UserDetailId"] != DBNull.Value ? Convert.ToInt64(dr["UserDetailId"]) : 0,
                                           AVGTrAmount = dr["AVGTrAmount"] != DBNull.Value ? Convert.ToDecimal(dr["AVGTrAmount"]) : 0.0m,
                                           BasketDataCount = dr["BasketDataCount"] != DBNull.Value ? Convert.ToInt32(dr["BasketDataCount"]) : 0,
                                          FirstName = dr["FirstName"].ToString() ?? string.Empty,
                                           LastName = dr["LastName"].ToString() ?? string.Empty,
                                          
                                           TotalRecords = dr["TotalRecords"] != DBNull.Value ? Convert.ToInt32(dr["TotalRecords"]) : 0,
                                           PreferredStore = dr["PreferredStore"].ToString() ?? string.Empty,
                                           
                                           
                                       }).ToList();
                        return advancedSearchShoppers;
                    }





                }

            }


            return null;
        }

        public async Task<List<FindShopperByUPCsList>> GetFindShopperByUPCs(string clientName, string? UPC, int NoOfCoupons, DateTime? FromDate, DateTime? ToDate)
        {
            List<FindShopperByUPCsList> findShopperByUPCsLists = new List<FindShopperByUPCsList>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_SHOPPERS_PURCHASED_PRODUCTS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@UPC",UPC),
                        new SqlParameter("@NoOfCoupons",NoOfCoupons),
                
                        new SqlParameter("@FromDate", FromDate ),
                        new SqlParameter("@ToDate",ToDate ?? null),
                        

                    };



                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                        findShopperByUPCsLists = (from dr in result
                                       select new  FindShopperByUPCsList
                                       {
                                          
                                           FIRSTNAME = dr["FIRSTNAME"].ToString() ?? string.Empty,
                                           LASTNAME = dr["LASTNAME"].ToString() ?? string.Empty,
                                           USERNAME = dr["USERNAME"].ToString() ?? string.Empty,
                                           USERDETAILID = dr["USERDETAILID"] != DBNull.Value ? Convert.ToInt32(dr["USERDETAILID"]) : 0,
                                           
                                           PREFERREDSTORE = dr["PREFERREDSTORE"].ToString() ?? string.Empty,
                                          
                                          TRANSACTIONAMOUNT = dr["TRANSACTIONAMOUNT"] != DBNull.Value ? Convert.ToDecimal(Convert.ToDecimal(dr["TRANSACTIONAMOUNT"]).ToString("F2")) : 0.00m,
                                       }).ToList();
                        return findShopperByUPCsLists;
                    }





                }

            }


            return null;
        }

        public async Task<List<ProductsDetails>> GetProductDetailsupc(string clientName, string? productCode, string? productName, int productCategoryId, bool? isMajorDepartment)
        {
            List<ProductsDetails> productsDetails = new List<ProductsDetails>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_FIND_PRODUCTS";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@ProductCode",string.IsNullOrEmpty(productCode) ? "" : productCode),
                        new SqlParameter("@ProductName",string.IsNullOrEmpty(productName) ? "" : productName),

                        new SqlParameter("@ProductCategoryId", productCategoryId ),
                        new SqlParameter("@IsMajorDepartment",isMajorDepartment),


                    };



                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                        productsDetails = (from dr in result
                                                  select new ProductsDetails
                                                  {

                                                      ProductId = dr["ProductId"] != DBNull.Value ? Convert.ToInt32(dr["ProductId"]) : 0,
                                                       ProductCode = dr["ProductCode"].ToString() ?? string.Empty,
                                                      ProductCategoryId = dr["ProductCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["ProductCategoryId"]) : 0,
                                                      ProductName = dr["ProductName"].ToString() ?? string.Empty,
                                                      Description = dr["Description"].ToString() ?? string.Empty,
                                                     
                                                     

                                                     SalePrice = dr["SalePrice"] != DBNull.Value ? Convert.ToDecimal(Convert.ToDecimal(dr["SalePrice"]).ToString("F2")) : 0.00m,
                                                      ProductCategoryName = dr["ProductCategoryName"].ToString() ?? string.Empty,
                                                      ProductImage = dr["ProductImage"].ToString() ?? string.Empty,
                                                  }).ToList();
                        return productsDetails;
                    }





                }

            }


            return null;
        }

        public async Task<List<AllShoppersGroups>> GetSearchAndCountDetails(string clientName, int minDays, int maxDays, string groupName)
        {
            List<AllShoppersGroups> allShoppersGroups = new List<AllShoppersGroups>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_GET_SEARCH_COUNT";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MinDays",minDays),
                        new SqlParameter("@MaxDays",maxDays),

                        new SqlParameter("@GroupName",groupName ?? "" ),
                        


                    };



                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                        allShoppersGroups = (from dr in result
                                             select new AllShoppersGroups
                                             {
                                                 GroupName = dr["GroupName"].ToString() ?? string.Empty,
                                                 GroupDetails = dr["GroupDetails"].ToString() ?? string.Empty,
                                                 GroupID = dr["GroupID"] != DBNull.Value ? Convert.ToInt32(dr["GroupID"]) : 0,
                                                 CreatedOn = Convert.ToDateTime(dr["CreatedOn"]).ToString("yyyy/MM/dd"),
                                                 TotalShoppers = dr["TotalShoppers"] != DBNull.Value ? Convert.ToInt32(dr["TotalShoppers"]) : 0,

                                               
                                                TopicARN = dr["TopicARN"].ToString() ?? string.Empty,



                                                
                                             }).ToList();
                        return allShoppersGroups;
                    }





                }

            }

            return null;
        }

        public async Task<List<AllShoppersGroups>> GetSearchAndCreateGroup(string clientName, int minDays, int maxDays, string groupName)
        {
            List<AllShoppersGroups> allShoppersGroups = new List<AllShoppersGroups>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_SEARCH_CREATE_GROUP";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MinDays",minDays),
                        new SqlParameter("@MaxDays",maxDays),

                        new SqlParameter("@GroupName",groupName ?? "" ),



                    };
                   



                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                   
                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                        allShoppersGroups = (from dr in result
                                             select new AllShoppersGroups
                                             {
                                                 GroupName = dr["GroupName"].ToString() ?? string.Empty,
                                                 GroupDetails = dr["GroupDetails"].ToString() ?? string.Empty,
                                                 GroupID = dr["GroupID"] != DBNull.Value ? Convert.ToInt32(dr["GroupID"]) : 0,
                                                 CreatedOn = Convert.ToDateTime(dr["CreatedOn"]).ToString("yyyy/MM/dd"),
                                                 TotalShoppers = dr["TotalShoppers"] != DBNull.Value ? Convert.ToInt32(dr["TotalShoppers"]) : 0,


                                                 TopicARN = dr["TopicARN"].ToString() ?? string.Empty,




                                             }).ToList();
                        return allShoppersGroups;
                    }





                }

            }
            return null;
        }

        public async Task<int> AddGroupWithSearchDetails(string clientName, int minDays, int maxDays, string groupName)
        {
          
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_SEARCH_CREATE_GROUP";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MinDays",minDays),
                        new SqlParameter("@MaxDays",maxDays),

                        new SqlParameter("@GroupName",groupName ?? "" ),



                    };
                  


                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.InsertTable(storedProcName, parameters);
                     if(result > 1)
                    {
                        return 1;
                    }




                }

            }
            return 0;
        }

        public async Task<int> UploadShoppersToGroupsWithFile(string clientName, string groupName, string memberNumbers)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "PROC_CUSTOM_UPLOAD_SHOPPERS_GROUPS_MEMBERNUMBER";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@GroupName",groupName ?? string.Empty),
                        new SqlParameter("@MemberNumbers",memberNumbers ?? string.Empty),

                     



                    };



                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.InsertTable(storedProcName, parameters);
                    if ((result > 1) || (result == -1))
                    {
                        return 1;
                    }




                }

            }
            return 0;
        }

        public async Task<List<SearchCoupons>> FindCoupons(string clientName, int newsCategoryId, DateTime? valid, DateTime? expires)
        {
            List<SearchCoupons> searchCoupons = new List<SearchCoupons>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };


                    // GetSSNewsWithRedeemCountAndClips

                    //var storedProcName = "SEARCH_COUPON_DETAILS";
                    var storedProcName = "GetSSNewsWithRedeemCountAndClips";
                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NewsCategoryID",newsCategoryId),
                       
                        new SqlParameter("@ValidFromDate",valid),

                        new SqlParameter("@ExpiresOn",expires),


                    };




                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);

                    var result = await sqlHelpers.GetMultipleRows(storedProcName, parameters);

                    if (result.Count > 0)
                    {
                        searchCoupons = (from dr in result
                                         select new SearchCoupons
                                         {
                                             NewsID = dr["NewsID"] != DBNull.Value ? Convert.ToInt32(dr["NewsID"]) : 0,
                                             NewsCategoryID = dr["NewsCategoryID"] != DBNull.Value ? Convert.ToInt32(dr["NewsCategoryID"]) : 0,
                                             Title = dr["Title"].ToString() ?? string.Empty,
                                             Details = dr["Details"].ToString() ?? string.Empty,
                                             OtherDetails = dr["OtherDetails"].ToString() ?? string.Empty,
                                             ProductImage = dr["ProductImage"].ToString() ?? string.Empty,
                                             ImagePath = dr["ImagePath"].ToString() ?? string.Empty,

                                             // Safe DateTime conversion
                                             ValidFromDate = dr["ValidFromDate"] != DBNull.Value
                                                 ? Convert.ToDateTime(dr["ValidFromDate"]).ToString("yyyy/MM/dd hh:mm:ss tt")
                                                 : string.Empty,

                                             ExpiresOn = dr["ExpiresOn"] != DBNull.Value
                                                 ? Convert.ToDateTime(dr["ExpiresOn"]).ToString("yyyy/MM/dd hh:mm:ss tt")
                                                 : string.Empty,

                                             MFGShutOffDate = dr["MFGShutOffDate"] != DBNull.Value
                                                 ? Convert.ToDateTime(dr["MFGShutOffDate"]).ToString("yyyy/MM/dd hh:mm:ss tt")
                                                 : string.Empty,

                                             CategoryName = dr["CategoryName"].ToString() ?? string.Empty,

                                             ProductId = dr["ProductId"] != DBNull.Value ? Convert.ToInt32(dr["ProductId"]) : 0,
                                             Amount = dr["Amount"] != DBNull.Value ? Convert.ToDecimal(dr["Amount"]) : 0.00m,
                                             ProductName = dr["ProductName"].ToString() ?? string.Empty,
                                             IsItStoreSpecific = dr["IsItStoreSpecific"] != DBNull.Value ? Convert.ToBoolean(dr["IsItStoreSpecific"]) : false,
                                             ManufacturerCouponId = dr["ManufacturerCouponId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerCouponId"]) : 0,
                                             ProductQuantity = dr["ProductQuantity"] != DBNull.Value ? Convert.ToInt32(dr["ProductQuantity"]) : 0,
                                             IsFeatured = dr["IsFeatured"] != DBNull.Value ? Convert.ToBoolean(dr["IsFeatured"]) : false,
                                             IsItTargetSpecific = dr["IsItTargetSpecific"] != DBNull.Value ? Convert.ToBoolean(dr["IsItTargetSpecific"]) : false,
                                             IsRecurring = dr["IsRecurring"] != DBNull.Value ? Convert.ToBoolean(dr["IsRecurring"]) : false,
                                             DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(dr["DiscountAmount"]) : 0.00m,
                                             IsDiscountPercentage = dr["IsDiscountPercentage"] != DBNull.Value ? Convert.ToBoolean(dr["IsDiscountPercentage"]) : false,
                                             IsDealOftheWeek = dr["IsDealOftheWeek"] != DBNull.Value ? Convert.ToBoolean(dr["IsDealOftheWeek"]) : false,
                                             //CouponUniqueId = dr["CouponUniqueId"] != DBNull.Value
                                             //    ? Guid.Parse(dr["CouponUniqueId"].ToString().ToUpper())
                                             //    : null,

                                             SendNotification = dr["SendNotification"] != DBNull.Value ? Convert.ToBoolean(dr["SendNotification"]) : false,
                                             CustomerID = dr["CustomerID"] != DBNull.Value ? Convert.ToInt32(dr["CustomerID"]) : 0,
                                             CustomerName = dr["CustomerName"].ToString() ?? string.Empty,
                                             CreateUserID = dr["CreateUserID"] != DBNull.Value ? Convert.ToInt32(dr["CreateUserID"]) : 0,
                                             UpdateUserID = dr["UpdateUserID"] != DBNull.Value ? Convert.ToInt32(dr["UpdateUserID"]) : 0,
                                             PUICode = dr["PUICode"].ToString() ?? string.Empty,
                                             CouponCode = dr["CouponCode"].ToString() ?? string.Empty,
                                             NCRPromotionCode = dr["NCRPromotionCode"].ToString() ?? string.Empty,

                                             // Safe DateTime conversion
                                             NCRPromotionCreatedDate = dr["NCRPromotionCreatedDate"] != DBNull.Value
                                                 ? Convert.ToDateTime(dr["NCRPromotionCreatedDate"]).ToString("yyyy/MM/dd hh:mm:ss tt")
                                                 : string.Empty,

                                             UpSellProductId = dr["UpSellProductId"] != DBNull.Value ? Convert.ToInt32(dr["UpSellProductId"]) : 0,
                                             UpSellProductQuantity = dr["UpSellProductQuantity"] != DBNull.Value ? Convert.ToInt32(dr["UpSellProductQuantity"]) : 0,
                                             RedeemCount = dr["RedeemCount"] != DBNull.Value ? Convert.ToInt32(dr["RedeemCount"]) : 0,
                                             ClipsCount = dr["ClipsCount"] != DBNull.Value ? Convert.ToInt32(dr["ClipsCount"]) : 0,
                                             CouponLimit = dr["CouponLimit"] != DBNull.Value ? Convert.ToInt32(dr["CouponLimit"]) : 0,
                                             ParentNewsId = dr["ParentNewsId"] != DBNull.Value ? Convert.ToInt32(dr["ParentNewsId"]) : 0,
                                             OfferTypeId = dr["OfferTypeId"] != DBNull.Value ? Convert.ToInt32(dr["OfferTypeId"]) : 0,
                                             IsPosIntegrationEnabled = dr["IsPosIntegrationEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsPosIntegrationEnabled"]) : false,
                                         }).ToList();

                        return searchCoupons;
                    }





                }

            }
            return null;
        }

        public async Task<List<NewsCategories>> GetNewsCategories(string clientName)
        {
            List<NewsCategories> newsCategories = new List<NewsCategories>();
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "GetNewsCategories";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                  




                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);

                    var result = await sqlHelpers.GetMultipleRows(storedProcName, null);

                    if (result.Count > 0)
                    {
                        newsCategories = (from dr in result
                                         select new NewsCategories
                                         {

                                             NewsCategoryId = dr["NewsCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["NewsCategoryId"]) : 0,
                                          
                                             CategoryName = dr["CategoryName"].ToString() ?? string.Empty,

                                            

                                         }).ToList();
                        return newsCategories;
                    }





                }

            }
            return null;
        }

        public async Task<int> CreateBasketCoupon(string clientName, SaveBasketModel model)
        {
           
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "SaveCouponBasket";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
                    string validFromDateString = model.ValidFromDate < SqlDateTime.MinValue.Value
    ? null 
    : model.ValidFromDate.ToString("yyyy-MM-dd HH:mm:ss");

                    string expiresOnString = model.ExpiresOn < SqlDateTime.MinValue.Value
                        ? null
                        : model.ExpiresOn.ToString("yyyy-MM-dd HH:mm:ss");

                    var validFromDateParam = new SqlParameter("@ValidFromDate", SqlDbType.VarChar, 50)
                    {
                        Value = string.IsNullOrEmpty(validFromDateString) ? (object)DBNull.Value : validFromDateString
                    };

                    var expiresOnParam = new SqlParameter("@ExpiresOn", SqlDbType.VarChar, 50)
                    {
                        Value = string.IsNullOrEmpty(expiresOnString) ? (object)DBNull.Value : expiresOnString
                    };

                 



                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NewsID", model.NewsID),
    new SqlParameter("@NewsCategoryID",model.NewsCategoryID),
    new SqlParameter("@Title", model.Title),
    new SqlParameter("@Details", model.Details),
    new SqlParameter("@ImagePath", model.ImagePath),
    new SqlParameter("@ValidFromDate", model.ValidFromDate),
    new SqlParameter("@ExpiresOn", model.ExpiresOn),
      //validFromDateParam,
    //expiresOnParam,
    new SqlParameter("@SendNotification", model.SendNotification),
    new SqlParameter("@CustomerID", model.CustomerID),
    new SqlParameter("@CreateUserID", model.CreateUserID),
    new SqlParameter("@UpdateUserID", model.UpdateUserID),
    new SqlParameter("@PUICode", model.PUICode),
    new SqlParameter("@ProductId", model.ProductId),
    new SqlParameter("@Amount", model.Amount),
    new SqlParameter("@DiscountAmount", model.DiscountAmount),
    new SqlParameter("@IsDiscountPercentage", model.IsDiscountPercentage),
    new SqlParameter("@NCRPromotionCode", model.NCRPromotionCode),
    new SqlParameter("@IsItStoreSpecific", model.IsItStoreSpecific),
    new SqlParameter("@ManufacturerCouponId",model.ManufacturerCouponId ),
    new SqlParameter("@ProductQuantity", model.ProductQuantity),
    new SqlParameter("@UpSellProductId", model.UpSellProductId),
    new SqlParameter("@UpSellProductQuantity", model.UpSellProductQuantity),
    new SqlParameter("@IsFeatured", model.IsFeatured),
    new SqlParameter("@DeleteFlag", model.DeleteFlag),
    new SqlParameter("@IsItTargetSpecific", model.IsItTargetSpecific),
    new SqlParameter("@OtherDetails", model.OtherDetails),
    new SqlParameter("@IsRecurring", model.IsRecurring),
    new SqlParameter("@MfgShutOffDate", model.MfgShutOffDate),
    new SqlParameter("@IsDealOftheWeek", model.IsDealOftheWeek),
    new SqlParameter("@News_Id", model.News_ID), // For output parameters, this can be adjusted with `Direction`
    new SqlParameter("@DepartmentId", model.DepartmentId),
    new SqlParameter("@IsMajorDepartment", model.IsMajorDepartment),
    new SqlParameter("@StoreID", model.StoreID),
    new SqlParameter("@PageNumber", model.PageNumber),
    new SqlParameter("@PdfFileName", model.PdfFileName),
    new SqlParameter("@Id", model.Id),
    new SqlParameter("@StoreRouteId", model.StoreRouteId),
    new SqlParameter("@ClientStoreId", model.ClientStoreId),
    new SqlParameter("@RecurringStartDate", model.RecurringStartDate),
    new SqlParameter("@RecurringEndDate", model.RecurringEndDate),
    new SqlParameter("@RecurringTypeId", model.RecurringTypeId),
    new SqlParameter("@ClubIds", model.ClubIds),
    new SqlParameter("@GroupNames", model.GroupNames),
    new SqlParameter("@ClientStoreIds", model.ClientStoreIds)


                    };



                    var parameter = new SqlParameter[]
{
    new SqlParameter("@NewsID", 0),
    new SqlParameter("@NewsCategoryID", 4),
    new SqlParameter("@Title", "test 4"),
    new SqlParameter("@Details", ""),
    new SqlParameter("@ImagePath", ""),
    new SqlParameter("@ValidFromDate", DBNull.Value),
    new SqlParameter("@ExpiresOn", DBNull.Value),
    new SqlParameter("@SendNotification", DBNull.Value),
    new SqlParameter("@CustomerID", 9),
    new SqlParameter("@CreateUserID", 1),
    new SqlParameter("@UpdateUserID", 1),
    new SqlParameter("@PUICode", ""),
    new SqlParameter("@ProductId", 1),
    new SqlParameter("@Amount", 20),
    new SqlParameter("@DiscountAmount", 0.2),
    new SqlParameter("@IsDiscountPercentage", false),
    new SqlParameter("@NCRPromotionCode", ""),
    new SqlParameter("@IsItStoreSpecific", false),
    new SqlParameter("@ManufacturerCouponId", 9),
    new SqlParameter("@ProductQuantity", 2),
    new SqlParameter("@UpSellProductId", 2),
    new SqlParameter("@UpSellProductQuantity", 1),
    new SqlParameter("@IsFeatured", false),
    new SqlParameter("@DeleteFlag", 0),
    new SqlParameter("@IsItTargetSpecific", false),
    new SqlParameter("@OtherDetails", ""),
    new SqlParameter("@IsRecurring", false),
    new SqlParameter("@MfgShutOffDate", DBNull.Value),
    new SqlParameter("@IsDealOftheWeek", false),
    new SqlParameter("@News_Id", 0), // For output parameters, this can be adjusted with `Direction`
    new SqlParameter("@DepartmentId", "1"),
    new SqlParameter("@IsMajorDepartment", false),
    new SqlParameter("@StoreID", "4,6"),
    new SqlParameter("@PageNumber", 2),
    new SqlParameter("@PdfFileName", ""),
    new SqlParameter("@Id", 0),
    new SqlParameter("@StoreRouteId", ""),
    new SqlParameter("@ClientStoreId", 4),
    new SqlParameter("@RecurringStartDate", DBNull.Value),
    new SqlParameter("@RecurringEndDate", DBNull.Value),
    new SqlParameter("@RecurringTypeId", DBNull.Value),
    new SqlParameter("@ClubIds", ""),
    new SqlParameter("@GroupNames", ""),
    new SqlParameter("@ClientStoreIds", "")
};




                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.InsertTable(storedProcName, parameters);
                    if ((result > 1) || (result == -1))
                    {
                        return 1;
                    }




                }

            }
            return 0;

        }

        public async Task<int> SaveUPCPromotion(string clientName, SaveUPCPromotionsModel model)
        {
            var appconfigurations = new List<AppConfigurations>();
            appconfigurations = await _s3BucketHelpers.GetS3JsonFileData();
            if (appconfigurations.Count > 0 && appconfigurations != null)
            {
                var s3bucketData = appconfigurations?.FirstOrDefault(c => c.ClinetName == clientName);
                if (s3bucketData != null)
                {


                    // string decryptText = await _helpers.DecryptAes(s3bucketData.Password);
                    var password = s3bucketData.ClinetName == "Veritra RSA" ? s3bucketData.Password : await _helpers.DecryptAes(s3bucketData.Password);
                    var rsaDBCon = new RSADBConnection
                    {
                        DataSource = s3bucketData.DBInstanceName,
                        Database = s3bucketData.DBName,
                        UserId = s3bucketData.UserName,
                        //  Password =_helpers.DecryptPassword(s3bucketData.Password),
                        // Password = decryptText
                        Password = password,

                    };




                    var storedProcName = "SaveUPCPromotions";

                    string connectionString = await _helpers.ConnectionStringBuilder(rsaDBCon);
    //                string validFromDateString = model.ValidFromDate < SqlDateTime.MinValue.Value
    //? null
    //: model.ValidFromDate.ToString("yyyy-MM-dd HH:mm:ss");

    //                string expiresOnString = model.ExpiresOn < SqlDateTime.MinValue.Value
    //                    ? null
    //                    : model.ExpiresOn.ToString("yyyy-MM-dd HH:mm:ss");

    //                var validFromDateParam = new SqlParameter("@ValidFromDate", SqlDbType.VarChar, 50)
    //                {
    //                    Value = string.IsNullOrEmpty(validFromDateString) ? (object)DBNull.Value : validFromDateString
    //                };

    //                var expiresOnParam = new SqlParameter("@ExpiresOn", SqlDbType.VarChar, 50)
    //                {
    //                    Value = string.IsNullOrEmpty(expiresOnString) ? (object)DBNull.Value : expiresOnString
    //                };





                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NewsID", model.NewsID),
    new SqlParameter("@NewsCategoryID",model.NewsCategoryID),
    new SqlParameter("@Title", model.Title),
    new SqlParameter("@Details", model.Details),
    new SqlParameter("@ImagePath", model.ImagePath),
    new SqlParameter("@ValidFromDate", model.ValidFromDate),
    new SqlParameter("@ExpiresOn", model.ExpiresOn),
      //validFromDateParam,
    //expiresOnParam,
    new SqlParameter("@SendNotification", model.SendNotification),
    new SqlParameter("@CustomerID", model.CustomerID),
    new SqlParameter("@CreateUserID", model.CreateUserID),
    new SqlParameter("@UpdateUserID", model.UpdateUserID),
    new SqlParameter("@PUICode", model.PUICode),
    new SqlParameter("@ProductId", model.ProductId),
    new SqlParameter("@Amount", model.Amount),
    new SqlParameter("@DiscountAmount", model.DiscountAmount),
    new SqlParameter("@IsDiscountPercentage", model.IsDiscountPercentage),
    new SqlParameter("@NCRPromotionCode", model.NCRPromotionCode),
    new SqlParameter("@IsItStoreSpecific", model.IsItStoreSpecific),
    new SqlParameter("@ManufacturerCouponId",model.ManufacturerCouponId ),
    new SqlParameter("@ProductQuantity", model.ProductQuantity),
    new SqlParameter("@UpSellProductId", model.UpSellProductId),
    new SqlParameter("@UpSellProductQuantity", model.UpSellProductQuantity),
    new SqlParameter("@IsFeatured", model.IsFeatured),
    new SqlParameter("@DeleteFlag", model.DeleteFlag),
    new SqlParameter("@IsItTargetSpecific", model.IsItTargetSpecific),
    new SqlParameter("@OtherDetails", model.OtherDetails),
    new SqlParameter("@IsRecurring", model.IsRecurring),
    new SqlParameter("@MfgShutOffDate", model.MfgShutOffDate),
    new SqlParameter("@IsDealOftheWeek", model.IsDealOftheWeek),
    new SqlParameter("@News_Id", model.News_ID), // For output parameters, this can be adjusted with `Direction`
    new SqlParameter("@DepartmentId", model.DepartmentId),
    new SqlParameter("@IsMajorDepartment", model.IsMajorDepartment),
    new SqlParameter("@StoreID", model.StoreID),
    new SqlParameter("@PageNumber", model.PageNumber),
    new SqlParameter("@PdfFileName", model.PdfFileName),
    new SqlParameter("@Id", model.Id),
    new SqlParameter("@StoreRouteId", model.StoreRouteId),
    new SqlParameter("@ClientStoreId", model.ClientStoreId),
    new SqlParameter("@RecurringStartDate", model.RecurringStartDate),
    new SqlParameter("@RecurringEndDate", model.RecurringEndDate),
    new SqlParameter("@RecurringTypeId", model.RecurringTypeId),
    new SqlParameter("@ClubIds", model.ClubIds),
    new SqlParameter("@GroupNames", model.GroupNames),
    new SqlParameter("@ClientStoreIds", model.ClientStoreIds),
    new SqlParameter("@UPC", model.ClientStoreIds),
    new SqlParameter("@ProductName", model.ClientStoreIds),
    new SqlParameter("@Product_ID", model.ClientStoreIds),

  


                    };



               




                    SqlHelpers sqlHelpers = new SqlHelpers(connectionString);
                    var result = await sqlHelpers.InsertTable(storedProcName, parameters);
                    if ((result > 1) || (result == -1))
                    {
                        return 1;
                    }




                }

            }
            return 0;
        }
    }
}
