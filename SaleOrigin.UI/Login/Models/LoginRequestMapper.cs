using SaleOrigin.Business.Authentication.Login;


namespace SaleOrigin.UI.Login
{
    public static class LoginRequestMapper
    {
        public static LoginRequestDto ToDto(LoginRequestModel model)
        {
            return new LoginRequestDto
            {
                //will add more info for log 

                UserName = model.UserName,
                Password = model.Password
            };
        }
    }
}
