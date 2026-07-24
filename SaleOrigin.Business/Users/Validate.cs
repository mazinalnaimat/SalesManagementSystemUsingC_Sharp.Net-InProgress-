

using Konscious.Security.Cryptography;
using System;
using System.Text;

namespace SaleOrigin.Business.Users
{
    public static class Validate
    {
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            bool isValidate = false;
            byte[] salt = Convert.FromBase64String(storedSalt);

            var argon2 = new Argon2id(
                Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 2,
                Iterations = 4,
                MemorySize = 65536
            };

            byte[] hash = argon2.GetBytes(32);

            isValidate  = Convert.ToBase64String(hash) == storedHash;
            return isValidate;
        }
    }
}
