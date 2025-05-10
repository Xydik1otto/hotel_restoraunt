using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            var sql = """
            SELECT b.*, g.*, r.* 
            FROM hotel_bookings b
            JOIN hotel_guests g ON b.guest_id = g.guest_id
            JOIN hotel_rooms r ON b.room_id = r.room_id
            """;
            
            return await _unitOfWork.Connection.QueryAsync<Booking, Guest, HotelRoom, Booking>(
                sql,
                (booking, guest, room) =>
                {
                    booking.GuestId = Convert.ToInt32(guest.ToString());
                    booking.RoomId = Convert.ToInt32(room.ToString());
                    return booking;
                },
                splitOn: "guest_id,room_id");
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            var sql = """
            SELECT b.*, g.*, r.* 
            FROM hotel_bookings b
            JOIN hotel_guests g ON b.guest_id = g.guest_id
            JOIN hotel_rooms r ON b.room_id = r.room_id
            WHERE b.booking_id = @Id
            """;
            
            var result = await _unitOfWork.Connection.QueryAsync<Booking, Guest, HotelRoom, Booking>(
                sql,
                (booking, guest, room) =>
                {
                    booking.GuestId = Convert.ToInt32(guest.ToString());
                    booking.RoomId = Convert.ToInt32(room.ToString());
                    return booking;
                },
                new { Id = id },
                splitOn: "guest_id,room_id");
                
            return result.FirstOrDefault();
        }

        public async Task CreateBooking(Booking booking)
        {
            var sql = """
            INSERT INTO hotel_bookings 
            (guest_id, room_id, check_in_date, check_out_date, total_price, booking_status, payment_status) 
            VALUES 
            (@GuestId, @RoomId, @CheckInDate, @CheckOutDate, @TotalPrice, @BookingStatus, @PaymentStatus)
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, booking, _unitOfWork.Transaction);
        }

        public async Task UpdateBooking(Booking booking)
        {
            var sql = """
            UPDATE hotel_bookings SET 
            guest_id = @GuestId,
            room_id = @RoomId,
            check_in_date = @CheckInDate,
            check_out_date = @CheckOutDate,
            total_price = @TotalPrice,
            booking_status = @BookingStatus,
            payment_status = @PaymentStatus
            WHERE booking_id = @BookingId
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, booking, _unitOfWork.Transaction);
        }

        public async Task DeleteBooking(int id)
        {
            var sql = "DELETE FROM hotel_bookings WHERE booking_id = @Id";
            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<Booking>> GetActiveBookings()
        {
            var sql = """
            SELECT b.*, g.*, r.* 
            FROM hotel_bookings b
            JOIN hotel_guests g ON b.guest_id = g.guest_id
            JOIN hotel_rooms r ON b.room_id = r.room_id
            WHERE b.booking_status = 'Confirmed' 
            AND b.check_out_date >= CURDATE()
            """;
            
            return await _unitOfWork.Connection.QueryAsync<Booking, Guest, HotelRoom, Booking>(
                sql,
                (booking, guest, room) =>
                {
                    booking.GuestId = Convert.ToInt32(guest.ToString());
                    booking.RoomId = Convert.ToInt32(room.ToString());
                    return booking;
                },
                splitOn: "guest_id,room_id");
        }

        public async Task<IEnumerable<Guest>> GetAllGuests()
        {
            var sql = "SELECT * FROM hotel_guests ORDER BY last_name, first_name";
            return await _unitOfWork.Connection.QueryAsync<Guest>(sql);
        }

        public async Task<IEnumerable<HotelRoom>> GetAllRooms()
        {
            var sql = "SELECT * FROM hotel_rooms WHERE is_available = TRUE ORDER BY room_number";
            return await _unitOfWork.Connection.QueryAsync<HotelRoom>(sql);
        }
    }
}