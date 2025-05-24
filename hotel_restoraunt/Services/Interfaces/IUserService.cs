// hotel_restoraunt/Services/Interfaces/IUserService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UpdateUserStatusAsync(int userId, UserStatus status);
        
        // Додані методи для сумісності з існуючим кодом
        Task<bool> Login(string username, string password);
        void Logout();
        User GetCurrentUser();
        IEnumerable<User> GetAllUsers();
    }
}