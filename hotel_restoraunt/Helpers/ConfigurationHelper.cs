// hotel_restoraunt/Helpers/ConfigurationHelper.cs
using System;
using System.IO;
using System.Text.Json;

namespace hotel_restoraunt.Helpers
{
    public static class ConfigurationHelper
    {
        public static T LoadConfiguration<T>(string sectionName) where T : new()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(configPath))
                {
                    var jsonString = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<JsonElement>(jsonString);
                    
                    if (config.TryGetProperty(sectionName, out var sectionConfig))
                    {
                        return JsonSerializer.Deserialize<T>(sectionConfig.ToString());
                    }
                }
                
                // Повернути значення за замовчуванням, якщо секцію не знайдено
                return new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження конфігурації {sectionName}: {ex.Message}");
                return new T();
            }
        }
        
        public static void SaveConfiguration<T>(string sectionName, T config) where T : class
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                JsonElement rootElement;
                
                if (File.Exists(configPath))
                {
                    var jsonString = File.ReadAllText(configPath);
                    rootElement = JsonSerializer.Deserialize<JsonElement>(jsonString);
                }
                else
                {
                    rootElement = JsonSerializer.Deserialize<JsonElement>("{}");
                }
                
                // Перетворюємо rootElement в словник, щоб можна було модифікувати
                var configObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(rootElement.ToString());
                
                // Оновлюємо або додаємо секцію
                var configJson = System.Text.Json.JsonSerializer.Serialize(config);
                var configElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(configJson);
                configObject[sectionName] = configElement;
                
                // Зберігаємо оновлену конфігурацію
                var options = new JsonSerializerOptions { WriteIndented = true };
                var updatedJson = System.Text.Json.JsonSerializer.Serialize(configObject, options);
                File.WriteAllText(configPath, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження конфігурації {sectionName}: {ex.Message}");
                throw;
            }
        }
    }
}