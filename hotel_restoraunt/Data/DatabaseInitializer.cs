// hotel_restoraunt/Data/DatabaseInitializer.cs
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace hotel_restoraunt.Data
{
    public static class DatabaseInitializer
    {
        public static async Task<bool> InitializeDatabaseAsync()
        {
            try
            {
                var config = DatabaseConfig.LoadConfig();
                var connectionStringBuilder = new MySqlConnectionStringBuilder
                {
                    Server = config.Server,
                    Port = (uint)config.Port,
                    UserID = config.UserId,
                    Password = config.Password
                };
                
                // З'єднання без вказівки бази даних
                string connectionString = connectionStringBuilder.ConnectionString;
                
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    
                    // Перевіряємо наявність бази даних
                    bool dbExists = await CheckDatabaseExistsAsync(connection, config.Database);
                    
                    if (!dbExists)
                    {
                        // Створюємо базу даних
                        await CreateDatabaseAsync(connection, config.Database);
                    }
                    
                    // Перемикаємося на створену базу даних
                    connection.ChangeDatabase(config.Database);
                    
                    // Виконуємо скрипти ініціалізації таблиць
                    await ExecuteInitializationScriptsAsync(connection);
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка ініціалізації бази даних: {ex.Message}");
                throw;
            }
        }
        
        private static async Task<bool> CheckDatabaseExistsAsync(MySqlConnection connection, string databaseName)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'";
            
            var result = await command.ExecuteScalarAsync();
            return result != null;
        }
        
        private static async Task CreateDatabaseAsync(MySqlConnection connection, string databaseName)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci";
            
            await command.ExecuteNonQueryAsync();
        }
        
        private static async Task ExecuteInitializationScriptsAsync(MySqlConnection connection)
        {
            // Шлях до вбудованих ресурсів зі скриптами SQL
            string[] scripts = {
                "CreateTables.sql", 
                "SeedData.sql"
            };
            
            foreach (var scriptName in scripts)
            {
                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", scriptName);
                
                if (File.Exists(scriptPath))
                {
                    string script = await File.ReadAllTextAsync(scriptPath);
                    
                    // Розділяємо скрипт на окремі команди
                    string[] commands = script.Split(new[] { "GO", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (string commandText in commands)
                    {
                        if (!string.IsNullOrWhiteSpace(commandText))
                        {
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = commandText;
                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Скрипт {scriptName} не знайдено");
                }
            }
        }
    }
}