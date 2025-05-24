// hotel_restoraunt/Data/Repositories/RoomRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Repositories
{
    public class RoomRepository : DatabaseService, IRoomRepository
    {
        public async Task<IEnumerable<HotelRoom>> GetAllRooms()
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    SELECT hr.*, rt.Name as RoomType 
                    FROM HotelRooms hr
                    JOIN RoomTypes rt ON hr.RoomTypeId = rt.RoomTypeId";
                
                return await connection.QueryAsync<HotelRoom>(sql);
            }
        }
        
        public async Task<HotelRoom> GetRoomById(int id)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    SELECT hr.*, rt.Name as RoomType 
                    FROM HotelRooms hr
                    JOIN RoomTypes rt ON hr.RoomTypeId = rt.RoomTypeId
                    WHERE hr.RoomId = @Id";
                
                return await connection.QueryFirstOrDefaultAsync<HotelRoom>(sql, new { Id = id });
            }
        }
        
        public async Task<HotelRoom> GetRoomByNumber(string roomNumber)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    SELECT hr.*, rt.Name as RoomType 
                    FROM HotelRooms hr
                    JOIN RoomTypes rt ON hr.RoomTypeId = rt.RoomTypeId
                    WHERE hr.RoomNumber = @RoomNumber";
                
                return await connection.QueryFirstOrDefaultAsync<HotelRoom>(sql, new { RoomNumber = roomNumber });
            }
        }
        
        public async Task<IEnumerable<HotelRoom>> GetAvailableRooms()
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    SELECT hr.*, rt.Name as RoomType 
                    FROM HotelRooms hr
                    JOIN RoomTypes rt ON hr.RoomTypeId = rt.RoomTypeId
                    WHERE hr.IsAvailable = 1";
                
                return await connection.QueryAsync<HotelRoom>(sql);
            }
        }
        
        public async Task<int> AddRoom(HotelRoom room)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    INSERT INTO HotelRooms (RoomNumber, RoomTypeId, PricePerNight, Capacity, Description, IsAvailable)
                    VALUES (@RoomNumber, 
                        (SELECT RoomTypeId FROM RoomTypes WHERE Name = @RoomType), 
                        @PricePerNight, @Capacity, @Description, @IsAvailable);
                    SELECT LAST_INSERT_ID();";
                
                return await connection.ExecuteScalarAsync<int>(sql, room);
            }
        }
        
        public async Task<bool> UpdateRoom(HotelRoom room)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    UPDATE HotelRooms 
                    SET RoomNumber = @RoomNumber, 
                        RoomTypeId = (SELECT RoomTypeId FROM RoomTypes WHERE Name = @RoomType),
                        PricePerNight = @PricePerNight, 
                        Capacity = @Capacity, 
                        Description = @Description, 
                        IsAvailable = @IsAvailable 
                    WHERE RoomId = @RoomId";
                
                int rowsAffected = await connection.ExecuteAsync(sql, room);
                return rowsAffected > 0;
            }
        }
        
        public async Task<bool> DeleteRoom(int id)
        {
            using (var connection = CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "DELETE FROM HotelRooms WHERE RoomId = @Id", new { Id = id });
                return rowsAffected > 0;
            }
        }
        
        public async Task<bool> SetRoomAvailability(int id, bool isAvailable)
        {
            using (var connection = CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "UPDATE HotelRooms SET IsAvailable = @IsAvailable WHERE RoomId = @Id", 
                    new { Id = id, IsAvailable = isAvailable });
                return rowsAffected > 0;
            }
        }
    }
}