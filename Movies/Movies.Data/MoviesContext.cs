using Movies.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data
{
    public class MoviesContext:DbContext
    {
        public MoviesContext()
            :base("MoviesDb")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
