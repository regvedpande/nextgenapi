using Microsoft.Data.SqlClient;
using StoreApi.Services.Interfaces;
using System;
using System.Data;

namespace StoreApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Register(string username, string passwordHash, string role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("RegisterUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@Role", role);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void SaveToken(int userId, string token)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SaveToken", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Token", token);
                command.Parameters.AddWithValue("@ExpiresAt", DateTime.UtcNow.AddHours(1));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
