namespace hotel_restoraunt.Models
{
    public class RestorauntTable
    {
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public string LocationDescription { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}