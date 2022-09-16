using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Extensions.Configuration;

namespace Codecool.CodecoolShop.Daos.Implementations.Database
{
    public class SupplierDaoDatabase : ISupplierDao
    {
        private IConfiguration _config;
        public string _connectionString;
        public SupplierDaoDatabase(IConfiguration config)
        {
            this._config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }   
        public void Add(Supplier item)
        {
            const string cmdText = @"INSERT INTO Products (Id, Name, Description)
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
            const string cmdText = @"DELETE FROM Suppliers WHERE Id = @id";
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
        public Supplier Get(int id)
        {
            const string cmdText = @"SELECT * FROM Suppliers WHERE Id = @id";
            try
            {
                Supplier supplier = new Supplier();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    supplier.Id = (int)reader["Id"];
                    supplier.Name = (string)reader["Name"];
                    supplier.Description = (string)reader["Description"];
                    connection.Close();
                }
                return supplier;
            }
            catch (SqlException e)
            {
                throw new RuntimeWrappedException(e);
            }
        }
        public IEnumerable<Supplier> GetAll()
        {
            const string cmdText = @"SELECT * FROM Suppliers";
            try
            {
                var results = new List<Supplier>();
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
                        Supplier supplier = new Supplier();
                        supplier.Id = (int)reader["Id"];
                        supplier.Name = (string)reader["Name"];
                        supplier.Description = (string)reader["Description"];
                        results.Add(supplier);
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