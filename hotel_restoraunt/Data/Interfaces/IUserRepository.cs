// hotel_restoraunt/Data/Interfaces/IUserRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<int> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<bool> AuthenticateUser(string username, string password);
    }
}