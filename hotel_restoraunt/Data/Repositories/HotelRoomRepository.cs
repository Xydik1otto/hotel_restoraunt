using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hotel_restoraunt.Data.Repositories
{
    public class HotelRoomRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelRoomRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelRoom>> GetAllRooms()
        {
            var sql = "SELECT * FROM hotel_rooms";
            return await _unitOfWork.Connection.QueryAsync<HotelRoom>(sql);
        }

        public async Task<bool> AddRoom(HotelRoom room)
        {
            var sql = """
            INSERT INTO hotel_rooms 
            (room_number, room_type, price_per_night, capacity, description, is_available) 
            VALUES 
            (@RoomNumber, @RoomType, @PricePerNight, @Capacity, @Description, @IsAvailable)
            """;
            
            var affectedRows = await _unitOfWork.Connection.ExecuteAsync(
                sql, 
                room, 
                _unitOfWork.Transaction);
            
            return affectedRows > 0;
        }

        public async Task<bool> UpdateRoom(HotelRoom room)
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
            
            var affectedRows = await _unitOfWork.Connection.ExecuteAsync(
                sql, 
                room, 
                _unitOfWork.Transaction);
            
            return affectedRows > 0;
        }

        public async Task<bool> DeleteRoom(int id)
        {
            var sql = "DELETE FROM hotel_rooms WHERE room_id = @Id";
            var affectedRows = await _unitOfWork.Connection.ExecuteAsync(
                sql, 
                new { Id = id }, 
                _unitOfWork.Transaction);
            
            return affectedRows > 0;
        }
    }
}