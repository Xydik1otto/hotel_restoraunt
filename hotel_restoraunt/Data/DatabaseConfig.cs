// hotel_restoraunt/Data/DatabaseConfig.cs
using hotel_restoraunt.Helpers;
using hotel_restoraunt.Services.Interfaces;
using System;

namespace hotel_restoraunt.Data
{
    public class DatabaseConfig
    {
        private readonly ILoggerService _logger;
        
        public string Server { get; set; } = "localhost";
        public string Database { get; set; } = "hotel_restaurant_db";
        public string UserId { get; set; } = "root";
        public string Password { get; set; } = "";
        public int Port { get; set; } = 3306;
        
        public string ConnectionString => 
            $"Server={Server};Database={Database};Port={Port};User Id={UserId};Password={Password};";
        
        public DatabaseConfig(ILoggerService logger = null)
        {
            _logger = logger;
        }

        public DatabaseConfig()
        {
            throw new NotImplementedException();
        }

        public static DatabaseConfig LoadConfig(ILoggerService logger = null)
        {
            try
            {
                logger?.LogInfo("Завантаження конфігурації бази даних");
                var config = ConfigurationHelper.LoadConfiguration<DatabaseConfig>("DatabaseConfig");
                
                // Встановлення значень за замовчуванням, якщо відсутні
                if (string.IsNullOrEmpty(config.Server)) config.Server = "localhost";
                if (string.IsNullOrEmpty(config.Database)) config.Database = "hotel_restaurant_db";
                if (string.IsNullOrEmpty(config.UserId)) config.UserId = "root";
                if (config.Port <= 0) config.Port = 3306;
                
                logger?.LogInfo($"Конфігурація бази даних завантажена успішно: Сервер={config.Server}, База={config.Database}, Порт={config.Port}");
                return config;
            }
            catch (Exception ex)
            {
                logger?.LogError("Помилка при завантаженні конфігурації бази даних", ex);
                
                // При помилці повертаємо конфігурацію за замовчуванням
                return new DatabaseConfig(logger) 
                { 
                    Server = "localhost", 
                    Database = "hotel_restaurant_db", 
                    UserId = "root", 
                    Password = "", 
                    Port = 3306 
                };
            }
        }
        
        public static void SaveConfig(DatabaseConfig config, ILoggerService logger = null)
        {
            try
            {
                logger?.LogInfo("Збереження конфігурації бази даних");
                ConfigurationHelper.SaveConfiguration("DatabaseConfig", config);
                logger?.LogInfo("Конфігурація бази даних збережена успішно");
            }
            catch (Exception ex)
            {
                logger?.LogError("Помилка при збереженні конфігурації бази даних", ex);
                throw;
            }
        }
    }
}