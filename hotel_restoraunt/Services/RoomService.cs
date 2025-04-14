using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class RoomService : IRoomService
{
    private readonly List<Room> _rooms = new();

    public List<Room> GetAllRooms() => _rooms;

    public void AddRoom(Room room) => _rooms.Add(room);

    public void RemoveRoom(Room room) => _rooms.Remove(room);
}