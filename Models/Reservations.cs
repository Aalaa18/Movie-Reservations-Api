using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Reservations
    {
        [Key]
        public int id { get; set; }


        public Appuser users { get; set; }
        [ForeignKey("users")]
        public string user_id { get; set; }

        public DateTime reservedate { get; set; }
        public string reservedseat { get; set; }

        public Showtime showtime { get; set; }
        [ForeignKey("showtime")]
        public int showtime_id { get; set; }


        public Reservations( DateTime reservedate, string reservedseat, int showtime_id, string user_id)
        {
           
            this.reservedate = reservedate;
            this.reservedseat = reservedseat;
            this.showtime_id = showtime_id;
            this.user_id = user_id;
           
        }
    }
}
