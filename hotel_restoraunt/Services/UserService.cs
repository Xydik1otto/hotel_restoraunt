using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class UserService : IUserService
{
    private readonly List<User> _users;

    public UserService()
    {
        _users = new List<User>();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }
    // Додано метод для оновлення користувача
    public void UpdateUser(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            // Інші властивості
        }
    }

    // Додано метод для видалення користувача
    public void DeleteUser(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            _users.Remove(user);
        }
    }
}
