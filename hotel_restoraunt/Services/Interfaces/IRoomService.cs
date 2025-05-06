using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<HotelRoom>> GetAllRooms();
        Task<HotelRoom?> GetRoomById(int id);
        Task CreateRoom(HotelRoom room);
        Task UpdateRoom(HotelRoom room);
        Task DeleteRoom(int id);
    }
}