using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllGuests();
        Task<Guest?> GetGuestById(int id);
        Task<Guest?> GetGuestByPassport(string passportNumber);
        Task AddGuest(Guest guest); // Змінено з CreateGuest на AddGuest
        Task UpdateGuest(Guest guest);
        Task DeleteGuest(int id);
    }
}