using SaleOrigin.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleOrigin.Business.Authentication.Login
{
    public class LoginResultDto
    {
        public bool IsSuccess { get; private set; }

        public string Message { get; private set; }

        public UserDto User { get; private set; }

        public LoginResultDto(bool isSuccess, string message, UserDto user)
        {
            IsSuccess = isSuccess;
            Message = message;
            User = user;
        }
    }
}
