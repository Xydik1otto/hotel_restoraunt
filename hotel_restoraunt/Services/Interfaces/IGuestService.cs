using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces;

public interface IGuestService
{
    void AddGuest(Guest guest);
    Guest? FindGuestById(int id);
    List<Guest> GetAllGuests();
}