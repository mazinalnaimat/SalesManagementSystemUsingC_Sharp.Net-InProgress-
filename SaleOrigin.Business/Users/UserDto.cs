using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleOrigin.Business.Users
{
    public class UserDto
    {
        public int UserId { get; private set; }
        public int PersonId { get; private set; }
        public string UserName { get; private set; }
        public string Status { get; private set; }

        public UserDto(int userId, int personId, string userName, string status)
        {
            UserId = userId;
            PersonId = personId;
            UserName = userName;
            Status = status;
        }
    }

}
