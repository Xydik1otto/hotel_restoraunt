using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services;

public class RoomService: IRoomService
{
    private readonly List<HotelRoom> _rooms;

    public RoomService()
    {
        _rooms = new List<HotelRoom>();
    }

    public void AddRoom(HotelRoom hotelRoom)
    {
        _rooms.Add(hotelRoom);
    }

    public List<HotelRoom> GetAvailableRooms()
    {
        return _rooms.Where(r => r.IsAvailable).ToList();
    }

    public HotelRoom? FindRoomById(int id)
    {
        return _rooms.FirstOrDefault(r => r.Id == id);
    }
    public IEnumerable<HotelRoom> GetAllRooms()
    {
        return _rooms;
    }
}