using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IReservationService
    {
        List<Reservation> GetAllReservations();
        void CreateReservation(Reservation reservation);
        void CancelReservation(Reservation reservation);
    }
}