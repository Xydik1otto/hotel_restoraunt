using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IReservationService
    {
        void AddReservation(Reservation reservation);
        List<Reservation> GetAllReservations();
        Reservation? FindReservationById(int id);
    }
}