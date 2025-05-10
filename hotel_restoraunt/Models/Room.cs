using System;

namespace hotel_restoraunt.Models
{
    public class Room : EntityBase
    {
        public string RoomNumber { get; set; }
        public int Floor { get; set; }
        public string RoomType { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        // Інші властивості...
    }
}
