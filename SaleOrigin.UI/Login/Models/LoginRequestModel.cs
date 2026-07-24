using SaleOrigin.UI.User;

namespace SaleOrigin.UI.Login
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public LoginRequestModel(string userName, string password)
        { 
            UserName = userName;
            Password = password;
        }

    }
}
