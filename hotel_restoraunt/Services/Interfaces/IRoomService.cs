using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetAllRooms();
        void AddRoom(Room room);
        void RemoveRoom(Room room);
    }
}