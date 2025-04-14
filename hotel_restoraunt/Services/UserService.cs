using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new()
    {
        new User { Username = "admin", Password = "admin", Role = "admin" },
        new User { Username = "user", Password = "user", Role = "user" }
    };

    public User? Authenticate(string username, string password)
    {
        return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
