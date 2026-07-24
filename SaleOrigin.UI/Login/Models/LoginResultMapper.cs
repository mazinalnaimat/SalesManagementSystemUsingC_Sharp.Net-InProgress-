using SaleOrigin.Business.Authentication.Login;


namespace SaleOrigin.UI.Login
{
    public static class LoginResultMapper
    { 
        public static LoginResultModel ToModel(LoginResultDto dto)
        {
            if (dto == null)
                return null;

            return new LoginResultModel
            {
                IsSuccess = dto.IsSuccess,
                Message = dto.Message,
                User = SaleOrigin.UI.User.UserMapper.ToModel(dto.User)
            };
        }
    }
}
