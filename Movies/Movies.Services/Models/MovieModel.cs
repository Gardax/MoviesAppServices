using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movies.Models;

namespace Movies.Services.Models
{
    public class MovieModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string CoverUrl { get; set; }

        public double Rating { get; set; }
    }
}