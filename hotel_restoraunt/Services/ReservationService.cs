using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class ReservationService : IReservationService
{
    private readonly List<Reservation> _reservations = new();

    public void CreateReservation(Reservation reservation) => _reservations.Add(reservation);

    public List<Reservation> GetAllReservations() => _reservations;

    public void CancelReservation(Reservation reservation) => _reservations.Remove(reservation);
}