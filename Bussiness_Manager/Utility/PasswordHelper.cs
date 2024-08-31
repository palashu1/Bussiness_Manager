using System.Security.Cryptography;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Bussiness_Manager.Utility
{
    public static class PasswordHelper
    {
        //Generate a random salt
        public static string GenerateSalt(int size = 16)
        {
            var saltBytes = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Hash a password with a salt
        public static string HashPassword(string password, string salt)
        {
            // Convert the salt from Base64 string to bytes
            var saltBytes = Convert.FromBase64String(salt);

            // Hash the password with the salt using PBKDF2
            var hashedBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            // Convert the hashed bytes to a Base64 string
            return Convert.ToBase64String(hashedBytes);
        }

        // Verify a password against a hash
        public static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            var hash = HashPassword(password, salt);
            return hash == hashedPassword;
        }

        public static string FinalHashPassword(string password)
        {
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(password, salt);
            return hashedPassword;
        }

        public static string EncryptPassword(string Password)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(Password);
                string encryptedPassword = Convert.ToBase64String(storePassword);
                return encryptedPassword;
            }
            else
            {
                return null;
            }
        }

        public static bool DycryptPassword(string encryptPassword, string clientPassword)
        {
            if (!string.IsNullOrEmpty(encryptPassword))
            {
                byte[] encryptedPassword = Convert.FromBase64String(encryptPassword);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                if (clientPassword == decryptedPassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
    }
}

