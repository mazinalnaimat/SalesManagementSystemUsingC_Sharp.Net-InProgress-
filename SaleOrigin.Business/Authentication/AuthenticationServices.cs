using SaleOrigin.Business.Authentication.Login;
using SaleOrigin.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SaleOrigin.Business.Authentication
{
    public class AuthenticationServices
    {
        public LoginResultDto Login(LoginRequestDto request)
        {
            LoginResultDto loginResultDto = null;
            UserDto userDto = UserServices.GetUserByUserNameAndPassword(request.UserName, request.Password);
            bool isSuccess = true;
            string msg = "Login Successful";
            if (userDto == null)
            {
                isSuccess = false;
                msg = "username or password wrong!!!";
            }
            else if (userDto.Status != "Active")
            {
                isSuccess = false;
                msg = "username is inactive or deleted";
            }
            loginResultDto = new LoginResultDto(isSuccess, msg, userDto);

            return loginResultDto;
        }
    }
}
