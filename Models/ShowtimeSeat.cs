using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class ShowtimeSeat
    {
        [Key]
        public int id { get; set; }

        public Seat seats { get; set; }

        public Showtime showtime { get; set; }

        public bool istaken { get; set; }

        [ForeignKey("seats")]
        public int seat_id;

        [ForeignKey("showtime")]
        public int showtime_id { get; set; }


        public ShowtimeSeat(int seat_id, int showtime_id)
        {
            this.seat_id = seat_id;
            this.showtime_id = showtime_id;
            this.istaken = false;
        }

    }
}
