using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IRoomService
    {
        void AddRoom(Room room);
        List<Room> GetAvailableRooms();
        Room? FindRoomById(int id);
        IEnumerable<Room> GetAllRooms();
    }
}