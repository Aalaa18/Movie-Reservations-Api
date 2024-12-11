using MovieApi.DTO;

namespace MovieApi.Services
{
    public interface Ireservationsservices
    {
        
        List<string> show_reservation(string name);
        Task<string> MakeReservationAsync(ReservationDTO reservationDTO);
        Task<List<int>> GetAvailableSeatsAsync(int showtimeId);
        Task<string> CancelReservationByIdAsync(int reservationId);
    }
}