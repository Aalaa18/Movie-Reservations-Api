using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace MovieApi.Models
{
    public class ServedHall
    {
        public int Id { get; set; }
        public ICollection<Seat> seats { get; set; }
        public Movie movie { get; set; }
        [ForeignKey("movie")]
        public int movie_id { get; set; }
        public ICollection<Showtime> showtime { get; set; }
        public Hall hall { get; set; }
        [ForeignKey("hall")]
        public int hall_id { get; set; }


        public ServedHall(int hall_id, int movie_id)
        {
            this.hall_id = hall_id;
            this.movie_id = movie_id;
        }
    }
}
