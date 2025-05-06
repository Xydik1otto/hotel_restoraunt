using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            var sql = "SELECT * FROM users WHERE username = @Username AND password_hash = @Password";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<User>(
                sql, 
                new { Username = username, Password = password });
        }

        public async Task<bool> UserExists(string username)
        {
            var sql = "SELECT COUNT(1) FROM users WHERE username = @Username";
            var count = await _unitOfWork.Connection.ExecuteScalarAsync<int>(
                sql, 
                new { Username = username });
            return count > 0;
        }

        public async Task AddUser(User user)
        {
            var sql = """
            INSERT INTO users 
            (username, password_hash, role, full_name) 
            VALUES 
            (@Username, @PasswordHash, @Role, @FullName)
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, user, _unitOfWork.Transaction);
        }

        public async Task UpdateUser(User user)
        {
            var sql = """
            UPDATE users SET 
            username = @Username,
            password_hash = @PasswordHash,
            role = @Role,
            full_name = @FullName
            WHERE user_id = @UserId
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, user, _unitOfWork.Transaction);
        }

        public async Task DeleteUser(int id)
        {
            var sql = "DELETE FROM users WHERE user_id = @Id";
            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var sql = "SELECT * FROM users ORDER BY username";
            return await _unitOfWork.Connection.QueryAsync<User>(sql);
        }
    }
}