using SaleOrigin.Business.Users;
using System.Collections.Generic;
using System.Linq;


namespace SaleOrigin.UI.User
{
    public static class UserMapper
    {
        public static UserModel ToModel(UserDto dto)
        {
            if (dto == null)
                return null;

            return new UserModel
            {
                UserId = dto.UserId,
                PersonId = dto.PersonId,
                UserName = dto.UserName,
                Status = dto.Status
            };
        }

        public static List<UserModel> ToModelList(IEnumerable<UserDto> dtos)
        {
            if (dtos == null)
                return new List<UserModel>();

            return dtos.Select(ToModel).ToList();
        }
    }
}
