using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        // Тут можна додати специфічні методи для роботи з бронюваннями
    }
}
