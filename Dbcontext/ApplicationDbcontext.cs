using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Dbcontext
{
    public class ApplicationDbcontext : IdentityDbContext<Appuser>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<ServedHall> ServedHalls { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Appuser> Users { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<ShowtimeSeat> showtimeseats { get; set; }

        public ApplicationDbcontext(DbContextOptions options) : base (options){}



    }
}
