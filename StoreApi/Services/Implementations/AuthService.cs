using Microsoft.Data.SqlClient;
using StoreApi.Models;
using StoreApi.Services.Interfaces;
using System;
using System.Data;

namespace StoreApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly string _connectionString;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User Authenticate(string username, string passwordHash)
        {
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("AuthenticateUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = reader.GetInt32(0),
                            Role = reader.GetString(1)
                        };
                    }
                }
            }
            return user;
        }

        public string GetToken(int userId)
        {
            string token = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetToken", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        token = reader.GetString(0);
                    }
                }
            }
            return token;
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
