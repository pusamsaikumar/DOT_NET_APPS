using Amazon.S3.Model;
using Amazon.S3;
using Newtonsoft.Json;

namespace RSASupportAPI.Models
{
    public class S3BucketHelpers
    {
        private readonly IAmazonS3 _amazonS3;

        public S3BucketHelpers(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }

        public async Task<string> GetS3BucketObjectDetails(string bucketName, string key)
        {
            var response = await _amazonS3.GetObjectAsync(bucketName, key);
            using (var reader = new StreamReader(response.ResponseStream))
            {
                return await reader.ReadToEndAsync();
            }
        }
       public async Task<List<AppConfigurations>> S3JsonConfigData(string bucketName, string key)
        {
           
            var configData = new List<AppConfigurations>();
            var getS3objects = await GetS3BucketObjectDetails(bucketName,key);
            
             configData = JsonConvert.DeserializeObject<List<AppConfigurations>>(getS3objects);
            if (configData != null) { 
                return configData;
            }
            return null;
        }
        public async Task<bool> ValidateKeyPath(string bucketName, string folderName, string transactionDate, int storeId, string fileName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = "",
            };
            var s3BucketObjects = await _amazonS3.ListObjectsV2Async(request);
            var validatePath = s3BucketObjects.S3Objects.FirstOrDefault(x =>
            {
                return x.Key.Split("/")[0] == folderName &&
                  x.Key.Split("/")[1].Length > 1 && x.Key.Split("/")[1] == transactionDate &&
                  x.Key.Split("/")[2].Length > 2 && x.Key.Split("/")[2] == (storeId).ToString() &&
                  x.Key.Split("/")[3].Length > 3 && x.Key.Split("/")[3] == fileName;
            });
            if (validatePath != null)
            {
                return true;
            }
            return false;
        }
        public async Task<string> ValidateKeyPathAsync(string bucketName, string folderName, string transactionDate, int storeId, string fileName)
        {
            string key = string.Empty;
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = ""
            };
            var result = await _amazonS3.ListObjectsV2Async(request);
            var validPath = result.S3Objects.FirstOrDefault(x =>
            {
                return x.Key.Split("/")[0] == folderName &&
                    x.Key.Split("/")[1] == transactionDate &&
                    x.Key.Split("/")[2] == storeId.ToString() &&
                    x.Key.Split("/")[3] == fileName;
            });
            if (validPath != null)
            {
                key = validPath.Key;

            }
            return key;
        }

        public async Task<bool> ExistFolderPath(string bucketName, string folderPath)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = folderPath,
            };
            var result = await _amazonS3.ListObjectsV2Async(request);
            if (result.S3Objects.Count > 0 || result.CommonPrefixes.Count > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<string> ExistedFolderPathKey(string bucketName, string key)
        {
            string keyPath = string.Empty;
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = key,
                };
                var result = await _amazonS3.ListObjectsV2Async(request);

                if (result.S3Objects.Count > 0)
                {
                    //return keyPath = key;
                    return keyPath = result.Prefix;

                }
                else
                {
                    return keyPath;
                }

            }
            catch (Exception ex)
            {

            }
            return keyPath;

        }

        // Get s3 bucket configs data:
        public async Task<List<AppConfigurations>> GetS3JsonFileData()
        {
            //var bucketName = "rsasupportapiproject";
            //rsasupportapiproject/RSASupportConfigurations/AppConfigurations.json
            // rsasupportapi / RSASupportConfigurations / AppConfigurations.json
            // rsasupportapidemo/RSASupportConfigurations/AppConfigurations.json
            // rsasupportapiproject/RSASupportConfigurations/AppConfigurations.json
            var bucketName = "rsasupportapiproject";
            var s3FolderFilePath = "RSASupportConfigurations/AppConfigurations.json";
            var validatePath = await ExistedFolderPathKey(bucketName, s3FolderFilePath);
            var appconfigurations = new List<AppConfigurations>();
            if (validatePath != null)
            {
                var jsonData = await GetS3BucketObjectDetails(bucketName, s3FolderFilePath);
                appconfigurations = JsonConvert.DeserializeObject<List<AppConfigurations>>(jsonData);

             if (appconfigurations != null) { 
                return appconfigurations;
                }
            }

            return null;
        }



    }
}
