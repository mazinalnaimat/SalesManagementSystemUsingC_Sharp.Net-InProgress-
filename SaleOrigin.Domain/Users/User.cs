
using SaleOrigin.DataAccess.Users;
using SaleOrigin.Domain.Users;

namespace SaleOrigin.Domain.Users
{
    public class User
    {
        public int UserId { get; private set; }
        public int PersonId { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string PasswordSalt { get; private set; }
        public long UserPermissionBinaryValue { get; private set; }
        public UserStatus Status {  get; private set; }

        public User(
               int userId,
               int personId,
               string userName,
               string password,
               string passwordSalt,
               long userPermissionBinaryValue,
               UserStatus status)
        {
            UserId = userId;
            PersonId = personId;
            UserName = userName;
            Password = password;
            PasswordSalt = passwordSalt;
            UserPermissionBinaryValue = userPermissionBinaryValue;            
            Status = status;
        }
    }
}
