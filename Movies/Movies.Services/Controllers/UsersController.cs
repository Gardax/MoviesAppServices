using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Movies.Data;
using Movies.Models;
using Movies.Services.Models;
using System.Text;

namespace Movies.Services.Controllers
{
    public class UsersController : ApiController
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 30;
        public const int MinUsernameLength = 4;
        public const int MaxUsernameLength = 30;

        private const string ValidNameCharacters =
           "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";

        private const string ValidUsernameCharacters =
           "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";

        private const string SessionKeyChars =
           "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";

        private static readonly Random rand = new Random();

        private const int SessionKeyLength = 50;

        [HttpPost]
        [ActionName("register")] //api/users/register
        public HttpResponseMessage PostRegisterUser(UserModel model)
        {
            try
            {
                var dbContext = new MoviesContext();
                using (dbContext)
                {
                    this.ValidateUsername(model.Username);
                    this.ValidateFirstname(model.FirstName);
                    this.ValidateLastname(model.LastName);

                    
                    var user = dbContext.Users.FirstOrDefault(u => u.Username.ToLower() == model.Username.ToLower());

                    if (user != null)
                    {
                        throw new InvalidOperationException("Users exists");
                    }

                    user = new User()
                    {
                        Username = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        AuthCode = model.AuthCode,
                        IsAdmin = false
                    };

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();

                    user.SessionKey = this.GenerateSessionKey(user.Id);
                    dbContext.SaveChanges();

                    var loggedModel = new LoggedUserModel()
                    {
                        Username = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsAdmin = false,
                        SessionKey = user.SessionKey
                    };

                    var response = this.Request.CreateResponse(HttpStatusCode.Created,
                                              loggedModel);
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpPost]
        [ActionName("login")]  //api/users/login
        public HttpResponseMessage PostLoginUser(UserModel model)
        {
            try
            {
                ValidateUsername(model.Username);

                var context = new MoviesContext();
                using (context)
                {
                    var user = context.Users.FirstOrDefault(u => u.Username == model.Username
                        && u.AuthCode == model.AuthCode);

                    if (user == null)
                    {
                        throw new InvalidOperationException("Invalid username or password");
                    }
                    if (user.SessionKey == null)
                    {
                        user.SessionKey = this.GenerateSessionKey(user.Id);
                        context.SaveChanges();
                    }

                    var loggedModel = new LoggedUserModel()
                    {
                        Username = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsAdmin = user.IsAdmin,
                        SessionKey = user.SessionKey
                    };

                    var response = this.Request.CreateResponse(HttpStatusCode.Created,
                                        loggedModel);
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                         ex.Message);
                return response;
            }
        }

        [HttpPut]
        [ActionName("logout")]  //api/users/logout/{sessionKey}
        public HttpResponseMessage PutLogoutUser(string sessionKey)
        {
            try
            {
                var context = new MoviesContext();
                using (context)
                {
                    var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                    if (user == null)
                    {
                        throw new ArgumentException("Invalid user authentication.");
                    }

                    user.SessionKey = null;
                    context.SaveChanges();

                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        private void ValidateFirstname(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Firstname cannot be null");
            }
            else if (username.Length < MinNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Firstname must be at least {0} characters long",
                    MinNameLength));
            }
            else if (username.Length > MaxNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Firstname must be less than {0} characters long",
                    MaxNameLength));
            }
            else if (username.Any(ch => !ValidNameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Firstname must contain only Latin letters.");
            }
        }

        private void ValidateLastname(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Lastname cannot be null");
            }
            else if (username.Length < MinNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Lastname must be at least {0} characters long",
                    MinNameLength));
            }
            else if (username.Length > MaxNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Lastname must be less than {0} characters long",
                    MaxNameLength));
            }
            else if (username.Any(ch => !ValidNameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Lastname must contain only Latin letters.");
            }
        }

        private void ValidateUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }
            else if (username.Length < MinUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username must be at least {0} characters long",
                    MinUsernameLength));
            }
            else if (username.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username must be less than {0} characters long",
                    MaxUsernameLength));
            }
            else if (username.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Username must contain only Latin letters, digits .,_");
            }

        }

        private string GenerateSessionKey(int userId)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(userId);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }
            return skeyBuilder.ToString();
        }
    }
}
