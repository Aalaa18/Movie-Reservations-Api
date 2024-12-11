using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        public ICollection<ShowtimeSeat> showtimeseats { get; set; }
    }
}
