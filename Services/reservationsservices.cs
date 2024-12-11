using Microsoft.EntityFrameworkCore;
using MovieApi.Dbcontext;
using MovieApi.DTO;
using MovieApi.Models;
using System.Xml.Linq;

namespace MovieApi.Services
{
    public class reservationsservices : Ireservationsservices
    {
        private readonly ApplicationDbcontext _context;
        public reservationsservices(ApplicationDbcontext context)
        {
            _context = context;
        }

        public List<string> show_reservation(string name)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == name);
            if (user == null) return null;
            var res = _context.Reservations.Where(x => x.user_id == user.Id).ToList();
            var list = new List<string>();
            if (res.Count > 0)
            {
                foreach (var us in res)
                {

                    list.Add($"{user.UserName} made a reservation  at time: {us.reservedate} with seats {us.reservedseat}");
                }
            }

            return list;
        }

        public async Task<string> MakeReservationAsync(ReservationDTO reservationDTO)
        {
            // Find the user by username
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == reservationDTO.name);
            if (user == null)
            {
                return "User not found.";
            }

            // Find the showtime by its ID
            var show = await _context.Showtimes.SingleOrDefaultAsync(s => s.id == reservationDTO.showid);
            if (show == null)
            {
                return "Showtime not found.";
            }

            if (!show.is_active)
            {
                return "This showtime is no longer active.";
            }

            // Get all seats for the showtime
            var showtimeSeats = await _context.showtimeseats
                .Where(s => s.showtime_id == show.id)
                .ToListAsync();

            // Check if the selected seat is available
            var selectedSeat = showtimeSeats.SingleOrDefault(s => s.seat_id == reservationDTO.seatid);
            if (selectedSeat == null || selectedSeat.istaken)
            {
                return "The selected seat is either invalid or already taken.";
            }

            // Mark the seat as taken
            selectedSeat.istaken = true;

            // Create a new reservation
            var reservation = new Reservations(DateTime.Now, reservationDTO.seatid.ToString(), reservationDTO.showid, user.Id.ToString());
            
            

            await _context.Reservations.AddAsync(reservation);

            // Save changes
            await _context.SaveChangesAsync();

            return "Seat booked successfully.";
        }

        public async Task<List<int>> GetAvailableSeatsAsync(int showtimeId)
        {
            var show = await _context.Showtimes.SingleOrDefaultAsync(s => s.id == showtimeId);
            if (show == null || !show.is_active)
            {
                return new List<int>();
            }

            var availableSeats = await _context.showtimeseats
                .Where(s => s.showtime_id == show.id && !s.istaken)
                .Select(s => s.seat_id)
                .ToListAsync();

            return availableSeats;
        }

        public async Task<string> CancelReservationByIdAsync(int reservationId)
        {
            try
            {
                // Retrieve the reservation by its ID
                var reservation = await _context.Reservations.SingleOrDefaultAsync(r => r.id == reservationId);
                if (reservation == null)
                {
                    return "Reservation not found.";
                }

                // Get showtime details
                var showtime = await _context.Showtimes.SingleOrDefaultAsync(s => s.id == reservation.showtime_id);
                if (showtime == null || showtime.date < DateTime.Now)
                {
                    return "Cannot cancel a past or invalid showtime.";
                }

                // Cancel the reserved seats
                var showseat = await _context.showtimeseats
                    .SingleOrDefaultAsync(s => s.seat_id.ToString() == reservation.reservedseat && s.showtime_id == reservation.showtime_id);
                if (showseat != null)
                {
                    showseat.istaken = false;
                }

                // Remove the reservation
                _context.Reservations.Remove(reservation);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return "Reservation cancelled successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error occurred: {ex.Message}");
                return $"An error occurred: {ex.Message}";
            }
        }





    }
}
