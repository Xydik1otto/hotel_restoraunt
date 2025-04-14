using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces;

public interface IUserService
{
    User? Authenticate(string username, string password);
}
