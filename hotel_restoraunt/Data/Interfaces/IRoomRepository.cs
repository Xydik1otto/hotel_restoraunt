// hotel_restoraunt/Data/Interfaces/IRoomRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<HotelRoom>> GetAllRooms();
        Task<HotelRoom> GetRoomById(int id);
        Task<HotelRoom> GetRoomByNumber(string roomNumber);
        Task<IEnumerable<HotelRoom>> GetAvailableRooms();
        Task<int> AddRoom(HotelRoom room);
        Task<bool> UpdateRoom(HotelRoom room);
        Task<bool> DeleteRoom(int id);
        Task<bool> SetRoomAvailability(int id, bool isAvailable);
    }
}