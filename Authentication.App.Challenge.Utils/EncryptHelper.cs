using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Authentication.App.Challenge.Utils
{
    public class EncryptHelper
    {
        public static HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8
            ));
            
            return new HashSalt { Hash = encryptedPassw , Salt = salt };
        }
        
        public static bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                enteredPassword,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8
            ));
            
            return encryptedPassword == storedPassword;
        }
        
        public class HashSalt
        {
            public string Hash { get; set; }

            public byte[] Salt { get; set; }
        }
    }
}