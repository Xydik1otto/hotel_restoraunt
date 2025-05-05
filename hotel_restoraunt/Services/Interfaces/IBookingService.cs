using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces;

public interface IBookingService
{
    Task<Booking> CreateBooking(Booking booking);
    Task<List<Booking>> GetAllBookings();
    Task<Booking> GetBookingById(int id);
    Task DeleteBooking(int id);
}