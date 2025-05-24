using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<Booking?> GetBookingById(int id);
        Task CreateBooking(Booking booking);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(int id); // Змінено з CancelBooking на DeleteBooking
        Task<IEnumerable<Booking>> GetActiveBookings();
        Task<IEnumerable<Guest>> GetAllGuests(); // Додано новий метод
        Task<IEnumerable<HotelRoom>> GetAllRooms(); // Додано новий метод
    }
}