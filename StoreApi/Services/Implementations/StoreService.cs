using Microsoft.Data.SqlClient;
using StoreApi.Models;
using StoreApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace StoreApi.Services.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly string _connectionString;

        public StoreService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Store> GetStores(int pageNumber, int pageSize)
        {
            var stores = new List<Store>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetStores", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stores.Add(new Store
                        {
                            StoreId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Address = reader.GetString(2),
                            City = reader.GetString(3),
                            State = reader.GetString(4),
                            Country = reader.GetString(5),
                            PostalCode = reader.GetString(6),
                            PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Email = reader.IsDBNull(8) ? null : reader.GetString(8)
                        });
                    }
                }
            }
            return stores;
        }

        public Store GetStoreById(int id)
        {
            Store store = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetStoreById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StoreId", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        store = new Store
                        {
                            StoreId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Address = reader.GetString(2),
                            City = reader.GetString(3),
                            State = reader.GetString(4),
                            Country = reader.GetString(5),
                            PostalCode = reader.GetString(6),
                            PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Email = reader.IsDBNull(8) ? null : reader.GetString(8)
                        };
                    }
                }
            }
            return store;
        }

        public void CreateStore(Store store)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("CreateStore", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Name", store.Name);
                command.Parameters.AddWithValue("@Address", store.Address);
                command.Parameters.AddWithValue("@City", store.City);
                command.Parameters.AddWithValue("@State", store.State);
                command.Parameters.AddWithValue("@Country", store.Country);
                command.Parameters.AddWithValue("@PostalCode", store.PostalCode);
                command.Parameters.AddWithValue("@PhoneNumber", (object)store.PhoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object)store.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@CreatedBy", store.CreatedBy);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStore(int id, Store store)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UpdateStore", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StoreId", id);
                command.Parameters.AddWithValue("@Name", store.Name);
                command.Parameters.AddWithValue("@Address", store.Address);
                command.Parameters.AddWithValue("@City", store.City);
                command.Parameters.AddWithValue("@State", store.State);
                command.Parameters.AddWithValue("@Country", store.Country);
                command.Parameters.AddWithValue("@PostalCode", store.PostalCode);
                command.Parameters.AddWithValue("@PhoneNumber", (object)store.PhoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object)store.Email ?? DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteStore(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DeleteStore", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StoreId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
