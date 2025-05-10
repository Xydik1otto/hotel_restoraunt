// hotel_restoraunt/Data/DatabaseContext.cs
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace hotel_restoraunt.Data
{
    public class DatabaseContext
    {
        private readonly DatabaseConfig _config;
        
        public DatabaseContext()
        {
            _config = DatabaseConfig.LoadConfig();
        }
        
        public IDbConnection CreateConnection()
        {
            var connection = new MySqlConnection(_config.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        
        public async Task<int> SaveChangesAsync()
        {
            // У реалізації з Dapper це місце може бути використано для додаткової логіки
            // збереження змін або логування
            return await Task.FromResult(1);
        }
        
        public void TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    bool result = connection.State == ConnectionState.Open;
                    if (!result)
                    {
                        throw new Exception("Не вдалося відкрити з'єднання з базою даних");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка підключення до бази даних: {ex.Message}", ex);
            }
        }
    }
}