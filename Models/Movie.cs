using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string category { get; set; }
        public ICollection<ServedHall> hall { get; set; }


        public Movie(string Name, string Description, string category)
        {

            this.Name = Name;
            this.Description = Description;
            this.category = category;
        }
    }
}
