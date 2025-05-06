// Models/MenuItem.cs
namespace hotel_restoraunt.Models
{
    public class MenuItem
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        
        // Навігаційна властивість
        public MenuCategory Category { get; set; }
    }
}