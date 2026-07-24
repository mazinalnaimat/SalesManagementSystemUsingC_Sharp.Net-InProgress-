using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

using SaleOrigin.Domain.Users;
using SaleOrigin.DataAccess.Users;

namespace SaleOrigin.Business.Users
{
    public static class UserServices
    {
        public static UserDto GetUserByUserNameAndPassword(string userName, string password)
        {
            UserDto userDto = null;

            // 1. Look up the user
            User user = UserData.GetUserByUserName(userName);
            bool userExists = (user != null);

            // 2. Prepare variables for the verification step
            // Use the real user data if found; otherwise, use dummy placeholders
            string hashToVerify = userExists ? user.Password : "DummyHashToMatchAlgorithmLength";
            string saltToVerify = userExists ? user.PasswordSalt : "DummySalt123";

            // 3. ALWAYS run the slow hashing function
            // This forces identical execution times for existing and non-existing users
            bool passwordIsValid = Validate.VerifyPassword(password, hashToVerify, saltToVerify);

            // 4. Only convert to DTO if the user actually exists AND the password is correct
            if (userExists && passwordIsValid)
            {
                userDto = UserMapper.ToDto(user);
            }

            return userDto;
        }
    }
}
