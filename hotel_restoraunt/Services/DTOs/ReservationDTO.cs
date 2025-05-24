using System;

namespace hotel_restoraunt.Services.DTOs
{
    public class ReservationDTO
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}