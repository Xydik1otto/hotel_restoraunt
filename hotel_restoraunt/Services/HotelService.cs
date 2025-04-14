// Services/HotelService.cs
using hotel_restoraunt.Models;
using System.Collections.Generic;

namespace hotel_restoraunt.Services
{
    public class HotelService
    {
        private readonly List<Room> _rooms = new List<Room>();

        public void AddRoom(Room room) => _rooms.Add(room);
        public List<Room> GetAllRooms() => _rooms;
    }
}