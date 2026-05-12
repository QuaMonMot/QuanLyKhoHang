using System.Data;
using Microsoft.Data.SqlClient;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SqlConnectionFactory _factory;

        public AuthRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public User Login(string username, string password)
        {
            User user = null;

            using SqlConnection conn =
                _factory.CreateConnection();

            SqlCommand cmd =
                new SqlCommand(
                    "sp_Login",
                    conn
                );

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Username",
                username
            );

            cmd.Parameters.AddWithValue(
                "@Password",
                password
            );

            conn.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            if (reader.Read())
            {
                user = new User
                {
                    UserId =
                        Convert.ToInt32(
                            reader["UserId"]
                        ),

                    Username =
                        reader["Username"]
                        .ToString(),

                    Role =
                        Convert.ToInt32(
                            reader["Role"]
                        )
                };
            }

            return user;
        }
        public void Register(User user)
        {
            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd =
                    new SqlCommand(
                        "sp_Register",
                        conn
                    );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Username",
                    user.Username
                );

                cmd.Parameters.AddWithValue(
                    "@Password",
                    user.Password
                );

                cmd.Parameters.AddWithValue(
                    "@Role",
                    user.Role
                );

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProfile(int userId, string username, string password)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Users SET Username = @Username, Password = @Password WHERE UserId = @UserId",
                    conn
                );

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}