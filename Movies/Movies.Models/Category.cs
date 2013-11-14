using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public Category()
        {
            this.Movies=new HashSet<Movie>();
        }
    }
}
