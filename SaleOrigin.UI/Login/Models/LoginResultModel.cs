using SaleOrigin.UI.User;

namespace SaleOrigin.UI.Login
{
    public class LoginResultModel
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public UserModel User {  get; set; }
    }
}
