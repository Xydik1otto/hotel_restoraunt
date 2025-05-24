using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelRoom>> GetAllRooms()
        {
            var sql = "SELECT * FROM hotel_rooms";
            return await _unitOfWork.Connection.QueryAsync<HotelRoom>(sql);
        }

        public async Task<HotelRoom?> GetRoomById(int id)
        {
            var sql = "SELECT * FROM hotel_rooms WHERE room_id = @Id";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<HotelRoom>(sql, new { Id = id });
        }

        public async Task CreateRoom(HotelRoom room)
        {
            var sql = """
            INSERT INTO hotel_rooms 
            (room_number, room_type, price_per_night, capacity, description, is_available) 
            VALUES 
            (@RoomNumber, @RoomType, @PricePerNight, @Capacity, @Description, @IsAvailable)
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, room, _unitOfWork.Transaction);
        }

        public async Task UpdateRoom(HotelRoom room)
        {
            var sql = """
            UPDATE hotel_rooms SET 
            room_number = @RoomNumber,
            room_type = @RoomType,
            price_per_night = @PricePerNight,
            capacity = @Capacity,
            description = @Description,
            is_available = @IsAvailable
            WHERE room_id = @RoomId
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, room, _unitOfWork.Transaction);
        }

        public async Task DeleteRoom(int id)
        {
            var sql = "DELETE FROM hotel_rooms WHERE room_id = @Id";
            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }
    }
}