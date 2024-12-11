using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTO
{
    public class DtoNewuser
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string code { get; set; }
       // public string Role { get; set; }

     
    }
}

