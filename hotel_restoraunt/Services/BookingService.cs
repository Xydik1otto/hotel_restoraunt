using hotel_restoraunt.Data;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_restoraunt.Services;

public class BookingService : IBookingService
{
    private readonly DatabaseHelper _context;

    public BookingService(DatabaseHelper context)
    {
        _context = context;
    }

    public async Task<Booking> CreateBooking(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.HotelRoom)
            .ToListAsync();
    }

    public async Task<Booking> GetBookingById(int id)
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.HotelRoom)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
}