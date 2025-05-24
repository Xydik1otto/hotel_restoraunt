// hotel_restoraunt/Data/Repositories/BaseRepository.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using hotel_restoraunt.Data.Interfaces;

namespace hotel_restoraunt.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;
        protected readonly string _tableName;
        protected readonly string _idColumn;
        
        protected BaseRepository(DatabaseContext context, string tableName, string idColumn = "Id")
        {
            _context = context;
            _tableName = tableName;
            _idColumn = idColumn;
        }
        
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {_tableName}");
            }
        }
        
        public virtual async Task<T> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<T>(
                    $"SELECT * FROM {_tableName} WHERE {_idColumn} = @Id", new { Id = id });
            }
        }
        
        public virtual async Task<int> AddAsync(T entity)
        {
            using (var connection = _context.CreateConnection())
            {
                // Створення SQL-запиту на основі властивостей моделі
                var properties = entity.GetType().GetProperties();
                var columnNames = new List<string>();
                var parameterNames = new List<string>();
                
                foreach (var prop in properties)
                {
                    // Пропускаємо ідентифікатор, якщо він автоінкрементний
                    if (prop.Name.Equals(_idColumn, StringComparison.OrdinalIgnoreCase))
                        continue;
                    
                    columnNames.Add(prop.Name);
                    parameterNames.Add($"@{prop.Name}");
                }
                
                var sql = $"INSERT INTO {_tableName} ({string.Join(", ", columnNames)}) " +
                         $"VALUES ({string.Join(", ", parameterNames)}); " +
                         $"SELECT LAST_INSERT_ID();";
                
                return await connection.ExecuteScalarAsync<int>(sql, entity);
            }
        }
        
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            using (var connection = _context.CreateConnection())
            {
                // Створення SQL-запиту на основі властивостей моделі
                var properties = entity.GetType().GetProperties();
                var updateColumns = new List<string>();
                PropertyInfo idProperty = null;
                
                foreach (var prop in properties)
                {
                    if (prop.Name.Equals(_idColumn, StringComparison.OrdinalIgnoreCase))
                    {
                        idProperty = prop;
                        continue;
                    }
                    
                    updateColumns.Add($"{prop.Name} = @{prop.Name}");
                }
                
                if (idProperty == null)
                    throw new InvalidOperationException($"Властивість {_idColumn} не знайдена в моделі {typeof(T).Name}");
                
                var sql = $"UPDATE {_tableName} SET {string.Join(", ", updateColumns)} " +
                         $"WHERE {_idColumn} = @{_idColumn}";
                
                int rowsAffected = await connection.ExecuteAsync(sql, entity);
                return rowsAffected > 0;
            }
        }
        
        public virtual async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    $"DELETE FROM {_tableName} WHERE {_idColumn} = @Id", new { Id = id });
                return rowsAffected > 0;
            }
        }
        
        public virtual async Task<bool> ExistsAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(1) FROM {_tableName} WHERE {_idColumn} = @Id", new { Id = id });
                return result > 0;
            }
        }
    }
}