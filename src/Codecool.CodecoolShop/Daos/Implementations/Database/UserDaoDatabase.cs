using Codecool.CodecoolShop.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Codecool.CodecoolShop.Daos.Implementations.Database
{
    public class UserDaoDatabase : IUser
    {
        private IConfiguration _config;
        public string _connectionString;
        public UserDaoDatabase(IConfiguration config)
        {
            this._config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public void Add(User item)
        {   
            const string cmdText = @"INSERT INTO Users (Id, Name, Passward)
                         VALUES (@Id, @Name, @Passward)
                                    SELECT SCOPE_IDENTITY();";
            if (GetAll().Count() > 0)
            {
                item.Id = GetAll().MaxBy(x => x.Id).Id + 1;
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Name", item.Email);
                    cmd.Parameters.AddWithValue("@Passward", item.Password.ToString());
                    cmd.ExecuteScalar();

                }
                catch (SqlException e)
                {
                    throw new RuntimeWrappedException(e);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public int GetId(string email)
        {
            const string cmdText = @"SELECT Id FROM Users WHERE Name = @email";

            using (var connection = new SqlConnection(_connectionString))
            {
                int id = 0;
                try
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@email", email);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        id = (int)reader["Id"];
                    }
                }
                catch (SqlException e)
                {
                    throw new RuntimeWrappedException(e);
                }
                finally
                {
                    connection.Close();
                }
                return id;
            }
        }

        public User Get(int id)
        {
            const string cmdText = @"SELECT * FROM Users WHERE Id = @id";

            User user = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    user.Id = (int)reader["Id"];
                    user.Email = (string)reader["Name"];
                    user.Password = (string)reader["Password"];
                }
                catch (SqlException e)
                {
                    throw new RuntimeWrappedException(e);
                }
                finally
                {
                    connection.Close();
                }
                return user;
            }
        }

        internal bool UserExist(string email, string password)
        {
            const string cmdText = @"SELECT * FROM Users WHERE Name = @Name AND Passward = @Passward";

            User user = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@Name", email);
                    cmd.Parameters.AddWithValue("@Passward", password);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user.Id = (int)reader["Id"];
                        user.Email = (string)reader["Name"];
                        user.Password = (string)reader["Passward"];
                    }
                }
                catch (SqlException e)
                {
                    throw new RuntimeWrappedException(e);
                }
                finally
                {
                    connection.Close();
                }
                return (user.Email != null && user.Password != null)  ? true : false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            const string cmdText = @"SELECT * FROM Users";

            var results = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                        return results;

                    while (reader.Read())
                    {
                        User user = new User();
                        user.Id = (int)reader["Id"];
                        user.Email = (string)reader["Name"];
                        user.Password = (string)reader["Passward"];
                        results.Add(user);
                    }
                }
                catch (SqlException e)
                {
                    throw new RuntimeWrappedException(e);
                }
                finally
                {
                    connection.Close();
                }
                return results;

            }
        }

        public void Remove(int id)
        {/////implementarea gresita
            const string cmdText = @"SELECT * FROM Users WHERE Id = @id";

            User user = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new SqlCommand(cmdText, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    user.Id = (int)reader["Id"];
                    user.Email = (string)reader["Name"];
                    user.Password = (string)reader["Password"];
                }

                catch (SqlException e)
                {
                    throw new RuntimeWrappedException(e);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
