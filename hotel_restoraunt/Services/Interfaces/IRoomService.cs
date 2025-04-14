using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services
{
    public interface IRoomService
    {
        List<Room> GetAllRooms();
        Room GetRoomById(int id);
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int id);
    }
}