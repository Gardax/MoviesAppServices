﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class Watches
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
