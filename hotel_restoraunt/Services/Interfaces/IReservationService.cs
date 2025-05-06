using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservations();
        Task<Reservation?> GetReservationById(int id);
        Task CreateReservation(Reservation reservation);
        Task UpdateReservation(Reservation reservation);
        Task CancelReservation(int id);
        Task<IEnumerable<Reservation>> GetTodayReservations();
        Task<IEnumerable<Customer>> GetAllCustomers(); // Додано новий метод
        Task<IEnumerable<RestorauntTable>> GetAllTables(); // Додано новий метод
    }
}