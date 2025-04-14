using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Services.DTOs;

namespace hotel_restoraunt.Services
{
    public interface IReservationService
    {
        Task<bool> CreateReservation(ReservationDTO dto);
        Task<List<ReservationDTO>> GetUserReservations(int userId);
        Task CancelReservation(int reservationId);
    }
}