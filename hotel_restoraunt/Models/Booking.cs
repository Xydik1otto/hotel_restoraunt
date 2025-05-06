using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_restoraunt.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }  // Змінено з Id на BookingId для відповідності БД

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; } = "Confirmed";
        public string PaymentStatus { get; set; } = "Pending";

        [ForeignKey("Guest")]
        public int GuestId { get; set; }  // Змінено з UserId на GuestId
        public virtual Guest Guest { get; set; }

        [ForeignKey("HotelRoom")]
        public int RoomId { get; set; }
        public virtual HotelRoom Room { get; set; }  // Змінено назву властивості
    }
}