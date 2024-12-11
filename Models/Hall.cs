using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Hall
    {
        
            [Key]
            public int id { get; set; }

            public ICollection<ServedHall> servedhalls { get; set; }

            public Hall(int id)
            {

                this.id = id;
            }
        
    }
}
