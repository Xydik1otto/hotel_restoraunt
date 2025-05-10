using System;

namespace hotel_restoraunt.Models
{
    public class Table : EntityBase
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public bool IsReserved { get; set; }
        // Інші властивості...
    }
}
