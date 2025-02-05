
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace RSASupportAPI.Models
{
    public class Helpers
    {
        public Helpers()
        {
            
        }
        public async Task<string> ConnectionStringBuilder(RSADBConnection model)
        {
            string connectionString = "";
            try
            {
                connectionString = "Data Source =" + model.DataSource + ";Initial Catalog=" + model.Database + ";uid=" + model.UserId + ";Password=" + model.Password;

            }
            catch (Exception ex) {
                connectionString = "";
            }
            return await Task.FromResult(connectionString);
        }

        // Encrypt: AES 256
        public async Task<string> EncryptAes(string plainText)
        {
            byte[] key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");  // 32-byte key
            byte[] iv = Encoding.UTF8.GetBytes("abcdef9876543210");  // 16-byte IV

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                          await  swEncrypt.WriteAsync(plainText);
                        }
                    }

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);  // Return Base64 encoded string
                }
            }
        }

       
        // Generate salt async
        public async Task<byte[]> GenerateSalt(int size = 16)
        {
            byte[] data = new byte[size];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }
            return await Task.FromResult(data);

        }

        //  hashed password
 
public async Task<string> HashPasswordWithSaltAsync(string plainPassword, byte[] salt)
    {
        return await Task.Run(() =>
        {
            using (var sha256 = SHA256.Create())
            {
                // Combine password and salt
                var passwordBytes = Encoding.UTF8.GetBytes(plainPassword);
                var combinedBytes = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

                // Hash the combined bytes
                var hashBytes = sha256.ComputeHash(combinedBytes);

                // Return hash as Base64 string
                return Convert.ToBase64String(hashBytes);
            }
        });
    }

        // verifypassword
        public async Task<bool> VerifyPassword(string plainPassword, byte[] salt,string storedPass)
        {
            string computeHash = await HashPasswordWithSaltAsync(plainPassword, salt);
           return await Task.FromResult(computeHash == storedPass);   

        }


    // Decrypt AES: 256
    public async Task<string> DecryptAes(string base64CipherText)
        {
            byte[] key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");  // 32-byte key
            byte[] iv = Encoding.UTF8.GetBytes("abcdef9876543210");  // 16-byte IV

            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(base64CipherText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(cipherTextBytes))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var msPlain = new MemoryStream())
                            {
                                await csDecrypt.CopyToAsync(msPlain);
                                byte[] decryptedBytes = msPlain.ToArray();
                                return Encoding.UTF8.GetString(decryptedBytes);  // Return decrypted string
                            }
                        }
                    }
                }
            }
            catch (CryptographicException cryptoEx)
            {
                throw new Exception("Decryption failed: " + cryptoEx.Message);
            }
        }

   

        // Encrypt password: Base64
        public string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "";
            }
            else
            {
                byte[] base64Password =  ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedPassword = Convert.ToBase64String(base64Password);
                return encryptedPassword;
            }
        }

        // Decrypt Password: Base64
        public string DecryptPassword(string password) {
            if (string.IsNullOrEmpty(password)) {
                return "";
            }
            else
            {
                byte[] encryptedPassword = Convert.FromBase64String(password);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                return decryptedPassword;
            }
        }


        public List<string> SanitizeAndSplit(string input)
        {
            //var sanitizedValue = input;

            //// Try parsing scientific notation to normal number format
            //if (input.Contains("E+"))
            //{
            //    // Attempt to parse the number in scientific notation and convert it to a plain string
            //    if (Decimal.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var decimalVal))
            //    {
            //        sanitizedValue = decimalVal.ToString("0"); // Convert to plain number format
            //    }
            //}

            //// Split the sanitized value and return it as a list of strings
            //return sanitizedValue.Split(',').Select(u => u.Trim()).ToList();
            var sanitizedValue = input;

            // If the value contains a comma, split it into separate values
            return sanitizedValue.Split(',').Select(u => u.Trim()).ToList();

        }
    }
}
