using System;
using System.Data.SqlClient;

using SaleOrigin.Domain.Users;


namespace SaleOrigin.DataAccess.Users
{
    public static class UserData
    {
        public static User GetUserByUserName(string userName)
        {
            User user = null;
            SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserName = @userName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@userName", userName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string password = (string)reader["Password"];

                    int userId = (int)reader["UserId"];
                    int personId = (int) reader["PersonId"];
                    string passwordSalt = (string) reader["PasswordSalt"];
                    long userPermissionBinaryValue = (long) reader["UserPermissionBinaryValue"];
                    UserStatus status = (UserStatus) reader["Status"];

                    user = new User(userId, personId, userName, password, passwordSalt, userPermissionBinaryValue, status);


                }

                reader.Close();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return user;
        }


    }
}
