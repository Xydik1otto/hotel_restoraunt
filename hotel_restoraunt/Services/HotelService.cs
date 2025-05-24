// Services/HotelService.cs
using hotel_restoraunt.Models;
using System.Collections.Generic;

namespace hotel_restoraunt.Services
{
    public class HotelService
    {
        private readonly List<HotelRoom> _rooms = new List<HotelRoom>();

        public void AddRoom(HotelRoom hotelRoom) => _rooms.Add(hotelRoom);
        public List<HotelRoom> GetAllRooms() => _rooms;
    }
}