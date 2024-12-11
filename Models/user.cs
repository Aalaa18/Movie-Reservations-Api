
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Appuser:IdentityUser
    {

        // public string Name { get; set; }
        
     
        public string pass { get; set; }
       //// public string email { get; set; }
        public string role { get; set; }

        public ICollection<Reservations> reservations { get; set; }
        public Appuser()
        {
            
        }
        public Appuser(string Name, string pass, string email)
        {

            this.UserName = Name;
            this.pass = pass;
            this.Email = email;
           

        }

        
    }
}
