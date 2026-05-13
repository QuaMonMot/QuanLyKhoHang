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
            using SqlConnection conn = _factory.CreateConnection();
            SqlCommand cmd = new SqlCommand("sp_Login", conn); // Đảm bảo sp_Login có SELECT FullName
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Username = reader["Username"].ToString(),
                    FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "", // Lấy FullName
                    Role = Convert.ToInt32(reader["Role"])
                };
            }
            return user;
        }

        //public User Login(string username, string password)
        //{
        //    User user = null;

        //    using SqlConnection conn =
        //        _factory.CreateConnection();

        //    SqlCommand cmd =
        //        new SqlCommand(
        //            "sp_Login",
        //            conn
        //        );

        //    cmd.CommandType =
        //        CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue(
        //        "@Username",
        //        username
        //    );

        //    cmd.Parameters.AddWithValue(
        //        "@Password",
        //        password
        //    );

        //    conn.Open();

        //    SqlDataReader reader =
        //        cmd.ExecuteReader();

        //    if (reader.Read())
        //    {
        //        user = new User
        //        {
        //            UserId =
        //                Convert.ToInt32(
        //                    reader["UserId"]
        //                ),

        //            Username =
        //                reader["Username"]
        //                .ToString(),

        //            Role =
        //                Convert.ToInt32(
        //                    reader["Role"]
        //                )
        //        };
        //    }

        //    return user;
        //}
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

        public void UpdateProfile(int userId, UpdateProfileDTO dto)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                string sql = @"UPDATE Users 
                       SET Username = @Username, 
                           FullName = @FullName, 
                           Email = @Email,
                           PhoneNumber = @Phone,
                           Address = @Address,
                           Gender = @Gender,
                           DateOfBirth = @DOB
                       WHERE UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Username", dto.Username);
                cmd.Parameters.AddWithValue("@FullName", (object)dto.FullName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)dto.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object)dto.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)dto.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", (object)dto.Gender ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DOB", (object)dto.DateOfBirth ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public User? GetById(int id)
        {
            using SqlConnection conn = _factory.CreateConnection();
            string sql = "SELECT * FROM Users WHERE UserId = @Id AND is_deleted = 0";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Username = reader["Username"].ToString(),
                    FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                    PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : "",
                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "",
                    Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : "",
                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : (DateTime?)null
                };
            }
            return null;
        }
    }
}