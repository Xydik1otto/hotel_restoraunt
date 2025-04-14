using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class RoomService: IRoomService
{
    private readonly List<Room> _rooms;

    public RoomService()
    {
        _rooms = new List<Room>();
    }

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }

    public List<Room> GetAvailableRooms()
    {
        return _rooms.Where(r => r.IsAvailable).ToList();
    }

    public Room? FindRoomById(int id)
    {
        return _rooms.FirstOrDefault(r => r.Id == id);
    }
    public IEnumerable<Room> GetAllRooms()
    {
        return _rooms;
    }
}