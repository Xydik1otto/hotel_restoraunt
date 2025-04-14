using System.Collections.Generic;
using hotel_restoraunt.Models.Enums;

namespace hotel_restoraunt.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public string RoomNumber { get; set; }
    }

}