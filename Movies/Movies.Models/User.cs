using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AuthCode { get; set; }

        public string SessionKey { get; set; }

        public bool IsAdmin { get; set; }

        public virtual ICollection<Movie> WatchedMovies { get; set; }

        public User()
        {
            this.WatchedMovies=new HashSet<Movie>();
        }
       
        
    }
}
