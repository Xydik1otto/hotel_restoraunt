using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces;

public interface IUserService
{
    void AddUser(User user);
    User? GetUserByEmail(string email);
    List<User> GetAllUsers();
    void UpdateUser(User user);
    void DeleteUser(int userId);
}
