using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Showtime
    {
        [Key]
        public int id { get; set; }
        public DateTime date { get; set; }

        public ServedHall servedhall { get; set; }
        [ForeignKey("servedhall")]
        public int show_hall_id { get; set; }

        public bool is_active { get; set; }

        public ICollection<ShowtimeSeat> showtimeseats { get; set; }

        public ICollection<Reservations> reservations { get; set; }

     
    }
}
