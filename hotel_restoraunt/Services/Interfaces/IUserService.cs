using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> Authenticate(string username, string password);
        Task<bool> UserExists(string username);
        Task AddUser(User user); // Змінено з CreateUser на AddUser
        Task UpdateUser(User user); // Додано новий метод
        Task DeleteUser(int id); // Додано новий метод
        Task<IEnumerable<User>> GetAllUsers(); // Додано новий метод
    }
}