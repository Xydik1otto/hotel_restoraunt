using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_restoraunt.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }
    public virtual HotelRoom HotelRoom { get; set; }
}