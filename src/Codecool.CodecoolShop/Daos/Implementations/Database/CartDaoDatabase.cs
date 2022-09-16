using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Codecool.CodecoolShop.Daos.Implementations.Database
{
    public class CartDaoDatabase : ICartDao
    {
        private IConfiguration _config;
        public string _connectionString;
        public CartDaoDatabase(IConfiguration config)
        {
            this._config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public void Add(Cart item)
        {
            const string cmdText = @"INSERT INTO Carts (ProductId, Amount, UserId)
                         VALUES (@ProductId, @Amount, @UserId)
                                    SELECT SCOPE_IDENTITY();";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@ProductId", item.product.Id);
                    cmd.Parameters.AddWithValue("@Amount", item.amount);
                    cmd.Parameters.AddWithValue("@UserId", item.userId);
                    cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }

        internal IEnumerable<Cart> GetByUserId(int userId)
        {
            const string cmdText = @"SELECT * FROM Carts WHERE UserId = @userId";
            try
            {
                var results = new List<Cart>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                        return results;

                    while (reader.Read())
                    {
                        Cart cart = new Cart();
                        cart.product = new ProductDaoDatabase(_config).Get((int)reader["ProductId"]);
                        cart.amount = (int)reader["Amount"];
                        cart.userId = (int)reader["UserId"];
                        results.Add(cart);
                    }
                    connection.Close();
                }
                return results;
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }

        public Cart Get(int id)
        {
            const string cmdText = @"SELECT * FROM Carts WHERE ProductId = @id";
            try
            {
                Cart cart = new Cart();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cart.product = new ProductDaoDatabase(_config).Get((int)reader["ProductId"]);
                        cart.amount = (int)reader["Amount"];
                        cart.userId = (int)reader["UserId"];
                    }
                    connection.Close();
                }
                return cart;
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public IEnumerable<Cart> GetAll()
        {
            const string cmdText = @"SELECT * FROM Carts";
            try
            {
                var results = new List<Cart>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                        return results;

                    while (reader.Read())
                    {
                        Cart cart = new Cart();
                        cart.product = new ProductDaoDatabase(_config).Get((int)reader["ProductId"]);
                        cart.amount = (int)reader["Amount"];
                        cart.userId = (int)reader["UserId"];
                        results.Add(cart);
                    }
                    connection.Close();
                }
                return results;
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public void Remove(int id)
        {
            const string cmdText = @"DELETE FROM Carts WHERE ProductId = @id";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public void RemoveAll()
        {
            const string cmdText = @"DELETE FROM Carts";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public void Update(int id, int userId)
        {
            const string cmdText = @"UPDATE Carts SET amount = amount+1 WHERE ProductId=@ProductId AND UserId = @UserId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
    }
}
