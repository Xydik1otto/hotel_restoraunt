// hotel_restoraunt/Models/Booking.cs
using System;

namespace hotel_restoraunt.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int BookingId { get => Id; set => Id = value; } // Для сумісності
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; } = 1;
        public int Children { get; set; } = 0;
        public string Status { get; set; } = "Підтверджено";
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = "Не оплачено";
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        // Властивості для відображення
        public string FormattedCheckInDate => CheckInDate.ToString("dd.MM.yyyy");
        public string FormattedCheckOutDate => CheckOutDate.ToString("dd.MM.yyyy");
        public string FormattedTotalAmount => string.Format("{0:C}", TotalAmount);
        public int NightsCount => (CheckOutDate - CheckInDate).Days;
        
        // Властивості для зв'язків
        public string GuestName { get; set; }
        public string RoomNumber { get; set; }
        public string RoomTypeName { get; set; }
    }
}