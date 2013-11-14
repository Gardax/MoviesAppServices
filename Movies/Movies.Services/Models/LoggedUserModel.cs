using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movies.Services.Models
{
    public class LoggedUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string SessionKey { get; set; }
        public bool IsAdmin { get; set; }
    }
}