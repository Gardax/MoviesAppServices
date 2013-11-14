using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movies.Services.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AuthCode { get; set; }
    }
}