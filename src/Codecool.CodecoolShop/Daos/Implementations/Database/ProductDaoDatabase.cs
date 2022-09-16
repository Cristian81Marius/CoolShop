using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Models;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Configuration;

namespace Codecool.CodecoolShop.Daos.Implementations.Database
{
    public class ProductDaoDatabase : IProductDao
    {
        private IConfiguration _config;
        public string _connectionString;
        private SupplierDaoDatabase _supplier;
        private ProductCategoryDaoDatabase _category;
        public ProductDaoDatabase(IConfiguration config)
        {
            this._config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
            _supplier = new SupplierDaoDatabase(_config);
            _category = new ProductCategoryDaoDatabase(_config);
        }
        public void Add(Product item)
        {
            const string cmdText = @"INSERT INTO Products (Id, Name, Description, Currency, DefaultPrice,CategoryId, SupplierId, ImageUrl, Amount)
                         VALUES (@Id, @Name, @Description, @Currency, @DefaultPrice, @CategoryId, @SupplierId , @ImageUrl, @Amount)
                            SELECT SCOPE_IDENTITY();";
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
                    cmd.Parameters.AddWithValue("@Currency", item.Currency);
                    cmd.Parameters.AddWithValue("@ImageUrl", item.ImageUrl);
                    cmd.Parameters.AddWithValue("@Amount", item.Amount);
                    cmd.Parameters.AddWithValue("@DefaultPrice", item.DefaultPrice);
                    cmd.Parameters.AddWithValue("@CategoryId", item.Currency);//ID
                    cmd.Parameters.AddWithValue("@SupplierId", item.Supplier);//ID

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
            //const string cmdText = @"DELETE FROM Products WHERE Id = @id AND Amount = 0";
            const string cmdText = @"DELETE FROM Products WHERE Id = @id";
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

        public Product Get(int id)
        {
            const string cmdText = @"SELECT * FROM Products WHERE Id = @id";
            try
            {
                Product product = new Product();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    product = SetProduct(product, reader);
                    connection.Close();
                }
                return product;
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public IEnumerable<Product> GetAll()
        {
            const string cmdText = @"SELECT * FROM Products";
            try
            {
                var results = new List<Product>();
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
                        Product product = new Product();
                        product = SetProduct(product, reader);
                        results.Add(product);
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
        public IEnumerable<Product> GetBy(Supplier supplier)
        {
            const string cmdText = @"SELECT * FROM Products WHERE SupplierId = @SupplierId";
            try
            {
                var results = new List<Product>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@SupplierId", supplier.Id);
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                        return results;
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product = SetProduct(product, reader);
                        results.Add(product);
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
        public IEnumerable<Product> GetBy(ProductCategory productCategory)
        {
            const string cmdText = @"SELECT * FROM Products WHERE CategoryId = @CategoryId";
            try
            {
                var results = new List<Product>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@CategoryId", productCategory.Id);

                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                        return results;

                    while (reader.Read())
                    {
                        Product product = new Product();
                        product = SetProduct(product, reader);
                        results.Add(product);
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
        public void Update(Product product)
        {
            const string cmdText = @"UPDATE Products 
                                    SET Amount = @amount 
                                    WHERE Id=@ProductId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@ProductId", product.Id);
                    cmd.Parameters.AddWithValue("@amount", product.Amount);
                    cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        protected Product SetProduct(Product product, SqlDataReader reader)
        {
            product.Id = (int)reader["Id"];
            product.Name = (string)reader["Name"];
            product.Amount = (int)reader["Amount"];
            product.ImageUrl = (string)reader["ImageUrl"];
            product.Description = (string)reader["Description"];
            product.Currency = (string)reader["Currency"];
            product.DefaultPrice = Convert.ToDecimal(reader["DefaultPrice"]);
            product.ProductCategory = _category.Get(Convert.ToInt32(reader["CategoryId"]));
            product.Supplier = _supplier.Get(Convert.ToInt32(reader["SupplierId"]));
            return product;
        }
    }
}
