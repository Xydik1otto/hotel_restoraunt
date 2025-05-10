// hotel_restoraunt/Models/MenuItem.cs
using System;

namespace hotel_restoraunt.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int MenuItemId { get => Id; set => Id = value; } // Властивість для сумісності
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        // Додаткові властивості для відображення
        public string FormattedPrice => string.Format("{0:C}", Price);
        public string CategoryName { get; set; }
        public string AvailabilityStatus => IsAvailable ? "Доступно" : "Недоступно";
    }
}