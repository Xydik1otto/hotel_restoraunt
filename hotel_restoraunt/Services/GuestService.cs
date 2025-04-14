using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class GuestService : IGuestService
{
    private readonly List<Guest> _guests;

    public GuestService()
    {
        _guests = new List<Guest>();
    }

    public void AddGuest(Guest guest)
    {
        _guests.Add(guest);
    }

    public Guest? FindGuestById(int id)
    {
        return _guests.FirstOrDefault(g => g.Id == id);
    }

    public List<Guest> GetAllGuests()
    {
        return _guests;
    }
}