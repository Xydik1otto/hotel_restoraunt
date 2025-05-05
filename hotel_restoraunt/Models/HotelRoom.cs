using System.Collections.Generic;
using hotel_restoraunt.Models.Enums;

namespace hotel_restoraunt.Models;

public class HotelRoom
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
}