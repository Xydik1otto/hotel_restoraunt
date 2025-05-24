// hotel_restoraunt/Data/Repositories/UserRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Repositories
{
    public class UserRepository : DatabaseService, IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<User>("SELECT * FROM Users");
            }
        }
        
        public async Task<User> GetUserById(int id)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE UserId = @Id", new { Id = id });
            }
        }
        
        public async Task<User> GetUserByUsername(string username)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Username = @Username", new { Username = username });
            }
        }
        
        public async Task<User> GetUserByEmail(string email)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Email = @Email", new { Email = email });
            }
        }
        
        public async Task<int> AddUser(User user)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    INSERT INTO Users (Username, PasswordHash, Email, FirstName, LastName, Role, IsActive)
                    VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, @Role, @IsActive);
                    SELECT LAST_INSERT_ID();";
                
                return await connection.ExecuteScalarAsync<int>(sql, user);
            }
        }
        
        public async Task<bool> UpdateUser(User user)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    UPDATE Users 
                    SET Username = @Username, 
                        Email = @Email, 
                        FirstName = @FirstName, 
                        LastName = @LastName, 
                        Role = @Role, 
                        IsActive = @IsActive 
                    WHERE UserId = @UserId";
                
                int rowsAffected = await connection.ExecuteAsync(sql, user);
                return rowsAffected > 0;
            }
        }
        
        public async Task<bool> DeleteUser(int id)
        {
            using (var connection = CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "DELETE FROM Users WHERE UserId = @Id", new { Id = id });
                return rowsAffected > 0;
            }
        }
        
        public async Task<bool> AuthenticateUser(string username, string password)
        {
            using (var connection = CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Username = @Username", new { Username = username });
                
                if (user == null) return false;
                
                // Тут потрібно використовувати безпечну перевірку хешу пароля
                // Наприклад, BCrypt.Net-Next
                // return BCrypt.Verify(password, user.PasswordHash);
                
                // Спрощена реалізація для прикладу:
                return user.PasswordHash == password;
            }
        }
    }
}