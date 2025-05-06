using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_restoraunt.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }  // Змінено з Id на ReservationId
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public int GuestsNumber { get; set; }
        public string SpecialRequests { get; set; }
        public string Status { get; set; } = "Confirmed";
        
        [ForeignKey("Guest")]
        public int GuestId { get; set; }
        public virtual Guest Guest { get; set; }
    
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual HotelRoom Room { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }  // Змінено з GuestId на CustomerId
        public virtual Customer Customer { get; set; }  // Змінено з Guest на Customer

        [ForeignKey("RestorauntTable")]
        public int TableId { get; set; }  // Змінено з RoomId на TableId
        public virtual RestorauntTable Table { get; set; }  // Змінено з HotelRoom на Table
    }
}