using System.Collections.Generic;
using hotel_restoraunt.Models.Enums;

namespace hotel_restoraunt.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public RoomType Type { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}