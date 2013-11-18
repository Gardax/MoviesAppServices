using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movies.Services.Models
{
    public class AddMovieModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string CoverUrl { get; set; }

        public double Rating { get; set; }

        public ICollection<CategoryModel> Categories { get; set; } 
    }
}