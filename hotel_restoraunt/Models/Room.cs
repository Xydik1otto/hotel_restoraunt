using System.Collections.Generic;
using hotel_restoraunt.Models.Enums;

namespace hotel_restoraunt.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }

        public Room(int id, string name, int capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }
    }

}