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

        public User? Login(string username, string password)
        {
            User? user = null;

            using SqlConnection conn = _factory.CreateConnection();
            using SqlCommand cmd = new SqlCommand("sp_Login", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            conn.Open();
             
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Username = reader["Username"].ToString(),
                    FullName = HasColumn(reader, "FullName") && reader["FullName"] != DBNull.Value
                        ? reader["FullName"].ToString()
                        : "",
                    Role = Convert.ToInt32(reader["Role"])
                };
            }

            return user;
        }

        public void Register(User user)
        {
            using SqlConnection conn = _factory.CreateConnection();
            using SqlCommand cmd = new SqlCommand("sp_Register", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Role", user.Role);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateProfile(int userId, UpdateProfileDTO dto)
        {
            using SqlConnection conn = _factory.CreateConnection();

            string sql = @"UPDATE Users
                       SET Username = @Username,
                           FullName = @FullName,
                           Email = @Email,
                           PhoneNumber = @Phone,
                           Address = @Address,
                           Gender = @Gender,
                           DateOfBirth = @DOB
                       WHERE UserId = @UserId";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Username", dto.Username);
            cmd.Parameters.AddWithValue("@FullName", (object?)dto.FullName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)dto.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object?)dto.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)dto.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object?)dto.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DOB", (object?)dto.DateOfBirth ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public User? GetById(int id)
        {
            using SqlConnection conn = _factory.CreateConnection();
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE UserId = @Id AND is_deleted = 0", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Username = reader["Username"].ToString(),
                    FullName = HasColumn(reader, "FullName") && reader["FullName"] != DBNull.Value
                        ? reader["FullName"].ToString()
                        : "",
                    Email = HasColumn(reader, "Email") && reader["Email"] != DBNull.Value
                        ? reader["Email"].ToString()
                        : "",
                    PhoneNumber = HasColumn(reader, "PhoneNumber") && reader["PhoneNumber"] != DBNull.Value
                        ? reader["PhoneNumber"].ToString()
                        : "",
                    Address = HasColumn(reader, "Address") && reader["Address"] != DBNull.Value
                        ? reader["Address"].ToString()
                        : "",
                    Gender = HasColumn(reader, "Gender") && reader["Gender"] != DBNull.Value
                        ? reader["Gender"].ToString()
                        : "",
                    DateOfBirth = HasColumn(reader, "DateOfBirth") && reader["DateOfBirth"] != DBNull.Value
                        ? Convert.ToDateTime(reader["DateOfBirth"])
                        : null
                };
            }

            return null;
        }

        private static bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
