using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class ReservationService : IReservationService
{
    private readonly List<Reservation> _reservations;

    public ReservationService()
    {
        _reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
    }

    public List<Reservation> GetAllReservations()
    {
        return _reservations;
    }

    public Reservation? FindReservationById(int id)
    {
        return _reservations.FirstOrDefault(r => r.Id == id);
    }
}