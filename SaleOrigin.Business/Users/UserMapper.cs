
using SaleOrigin.Domain.Users;

namespace SaleOrigin.Business.Users
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            if (user == null)
                return null;
            UserDto userDto = null;

            userDto = new UserDto(user.UserId, user.PersonId, user.UserName, user.Status.ToString());

            return userDto;
        }
    }
}
