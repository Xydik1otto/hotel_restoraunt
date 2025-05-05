using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IRoomService
    {
        void AddRoom(HotelRoom hotelRoom);
        List<HotelRoom> GetAvailableRooms();
        HotelRoom? FindRoomById(int id);
        IEnumerable<HotelRoom> GetAllRooms();
    }
}