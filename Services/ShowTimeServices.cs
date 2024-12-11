using Microsoft.EntityFrameworkCore;
using MovieApi.Dbcontext;
using MovieApi.DTO;
using MovieApi.Models;
using System;

namespace MovieApi.Services
{
    public class ShowTimeServices : IShowTimeServices
    {
        private readonly ApplicationDbcontext _context;

        public ShowTimeServices(ApplicationDbcontext context)
        {

            _context = context;

        }


        public async Task<List<string>> listshowtimes()
        {
            var showtimes = await _context.Showtimes.OrderBy(s => s.date).ToListAsync();

            var list = new List<string>();
            if (showtimes.Count > 0)
            {
                foreach (var showtime in showtimes)
                {
                    var rhalls = await _context.ServedHalls.SingleOrDefaultAsync(x => x.Id == showtime.show_hall_id);
                    var hall = await _context.Halls.SingleOrDefaultAsync(h => h.id == rhalls.hall_id);
                    var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == rhalls.movie_id);
                    list.Add($"showtime_id: {showtime.id} movie :{movie.Name}  movie type :{movie.category} at hall: {hall.id} at time : {showtime.date}");
                }
            }

            return list;
        }

        public async Task<string> AddShowTime(ShowtimeDTO showtimedto)
        {
            // Check if the movie exists
            var ismovie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == showtimedto.movie_id);
            if (ismovie == null)
            {
                return "Movie not found!";
            }

            // Check if the hall exists
            var ishall = await _context.Halls.FirstOrDefaultAsync(x => x.id == showtimedto.hall_id);
            if (ishall == null)
            {
                return "Hall not found!";
            }

            // Check for conflicting showtime
            var conflictingShowtime = await _context.Showtimes
                .FirstOrDefaultAsync(x => x.show_hall_id == showtimedto.hall_id && x.date == showtimedto.date);
            if (conflictingShowtime != null)
            {
                return "There's already a showtime for this hall at the specified date.";
            }

            // Check if the movie is already assigned to the hall
            var existingAssignment = await _context.ServedHalls
                .FirstOrDefaultAsync(x => x.movie_id == showtimedto.movie_id && x.hall_id == showtimedto.hall_id);

            if (existingAssignment != null)
            {
                return "This movie is already assigned to this hall.";
            }

            // Add a new ServedHall assignment
            var served = new ServedHall(ismovie.Id, ishall.id);
            await _context.ServedHalls.AddAsync(served);
            await _context.SaveChangesAsync();

            // Add the new showtime
            var showtime = new Showtime
            {
                date = showtimedto.date,
                is_active = true,
                show_hall_id = served.Id
            };
            await _context.AddAsync(showtime);
            await _context.SaveChangesAsync();

            return "Show Time Added";
        }


        public async Task<string> RemoveShowTime(int id)
        {

           
                var show = await _context.Showtimes.SingleOrDefaultAsync(showtimes => showtimes.id == id);
                if (show == null) return "in valid showtime id";

            var reservation = await _context.Reservations.Where(reservation => reservation.showtime_id == show.id).ToListAsync();
            if (reservation.Count != 0)
            {
                foreach (var item in reservation)
                {
                    _context.Reservations.Remove(item);
                    return "reservation are cancelled";
                }
               
            }


            _context.Showtimes.Remove(show);
            _context.SaveChanges();

            return "Removed";
        }

        public async Task<string> AddSeatsForShowtime(int id)
        {
            var show = _context.Showtimes.SingleOrDefault(showtimes => showtimes.id == id);

            if (show != null)
            {
                int seatnum = 10;

                for (int i = 1; i <= seatnum; i++)
                {
                    // Check if the seat exists in the Seats table
                    var existingSeat = _context.Seats.SingleOrDefault(seat => seat.Id == i);

                    if (existingSeat == null)
                    {
                        // If the seat does not exist, create it
                        var newSeat = new Seat(); // Do not set the Id
                        _context.Seats.Add(newSeat);
                        await _context.SaveChangesAsync();

                    }

                    // Now add the ShowtimeSeat
                    var seat = new ShowtimeSeat(i, show.id);
                    _context.showtimeseats.Add(seat);
                }

                await _context.SaveChangesAsync();
                return "Seats Added Successfully";
            }
            else
            {
                return null;
            }
        }




    }
}
