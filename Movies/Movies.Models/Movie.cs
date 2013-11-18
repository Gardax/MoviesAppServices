using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CoverUrl { get; set; }

        public double Rating { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Watches> WhachedBy { get; set; }

        public virtual ICollection<Category> Categories { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; }
 
        public Movie()
        {
            this.Comments=new HashSet<Comment>();
            this.Categories=new HashSet<Category>();
            this.Votes=new HashSet<Vote>();
            this.WhachedBy = new HashSet<Watches>();
        }
    }
}
