// Data/DatabaseContext.cs
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace hotel_restoraunt.Data
{
    public class DatabaseContext
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task InitDatabase()
        {
            using var connection = CreateConnection();
            await _initTables();
            
            async Task _initTables()
            {
                // Перевірка ініціалізації таблиць
                var sql = """
                          SELECT COUNT(*) FROM information_schema.tables 
                          WHERE table_schema = 'kursova' 
                          AND table_name IN ('hotel_rooms', 'hotel_guests', 'hotel_bookings');
                          """;
                
                var count = await connection.ExecuteScalarAsync<int>(sql);
                if (count < 3) throw new Exception("Database tables not properly initialized");
            }
        }
    }
}