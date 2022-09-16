using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Extensions.Configuration;


namespace Codecool.CodecoolShop.Daos.Implementations.Database
{
    public class ProductCategoryDaoDatabase : IProductCategoryDao
    {
        private IConfiguration _config;

        public string _connectionString;
        public ProductCategoryDaoDatabase(IConfiguration config)
        {
            this._config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public void Add(ProductCategory item)
        {
            const string cmdText = @"INSERT INTO ProductsCategory (Id, Name, Description)
                         VALUES (@Id, @Name, @Description) SELECT SCOPE_IDENTITY();";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Description", item.Description);

                    cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public void Remove(int id)
        {
            const string cmdText = @"DELETE FROM ProductsCategory WHERE Id = @id";
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
        public ProductCategory Get(int id)
        {
            const string cmdText = @"SELECT * FROM ProductsCategory WHERE Id = @id";
            try
            {
                ProductCategory productCategory = new ProductCategory();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    productCategory.Id = (int)reader["Id"];
                    productCategory.Name = (string)reader["Name"];
                    productCategory.Description = (string)reader["Description"];
                    connection.Close();
                }
                return productCategory;
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public IEnumerable<ProductCategory> GetAll()
        {
            const string cmdText = @"SELECT * FROM ProductsCategory";
            try
            {
                var results = new List<ProductCategory>();
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
                        ProductCategory productCategory = new ProductCategory();
                        productCategory.Id = (int)reader["Id"];
                        productCategory.Name = (string)reader["Name"];
                        productCategory.Description = (string)reader["Description"];
                        results.Add(productCategory);
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
    }
}
