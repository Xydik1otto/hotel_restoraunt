// hotel_restoraunt/Data/DatabaseService.cs
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;


namespace hotel_restoraunt.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        
        public DatabaseService()
        {
            var config = DatabaseConfig.LoadConfig();
            _connectionString = config.ConnectionString;
        }
        
        protected IDbConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        
        public void TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    // Тест підключення
                    bool result = connection.State == ConnectionState.Open;
                    Console.WriteLine($"Підключення до бази даних: {(result ? "успішно" : "не вдалося")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка підключення до бази даних: {ex.Message}");
                throw;
            }
        }
    }
}