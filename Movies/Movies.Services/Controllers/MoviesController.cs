using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Movies.Data;
using Movies.Models;
using Movies.Services.Models;

namespace Movies.Services.Controllers
{
    public class MoviesController : ApiController
    {
        [HttpPost]
        [ActionName("AddMovie")]
        public HttpResponseMessage AddMovie(string sessionKey, AddMovieModel movie)
        {
            try
            {
                var context = new MoviesContext();
                var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null || user.IsAdmin==false)
                {
                    throw new ArgumentException("You don't have permissions to add movies!");
                }

                var newMovie = new Movie()
                                   {
                                       Title = movie.Title,
                                       Description = movie.Description,
                                       CoverUrl = movie.CoverUrl
                                   };

                if (movie.Categories==null || movie.Categories.Count()==0)
                {
                    throw  new ArgumentException("Movie must have categories!");
                }

                foreach (var category in movie.Categories)
                {
                    var cat = context.Categories.FirstOrDefault(c => c.Name == category.Name);
                    if(cat==null)
                    {
                        context.Categories.Add(new Category()
                                                   {
                                                       Name = category.Name
                                                   });
                        context.SaveChanges();
                        cat = context.Categories.FirstOrDefault(c => c.Name == category.Name);
                    }
                    newMovie.Categories.Add(cat);
                }
                context.Movies.Add(newMovie);
                context.SaveChanges();

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpGet]
        [ActionName("GetTopMovies")]
        public HttpResponseMessage GetATopMovies(string sessionKey, int page=1)
        {
            try
            {
                var context = new MoviesContext();
                    var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                    if (user == null)
                    {
                        throw new ArgumentException("Invalid authentication!");
                    }

                    page = (page - 1)*20;
                    var moviesEntity = context.Movies.Where(m=>m.WhachedBy.All(w=>w.User.Id!=user.Id)).OrderByDescending(
                        m => m.Rating)
                        .Skip(page).Take(20).ToList();

                    var movies = from movie in moviesEntity
                                 select new SimpleMovieModel()
                                            {
                                                Id=movie.Id,
                                                Title = movie.Title,
                                                /*Description = movie.Description,
                                                CoverUrl = movie.CoverUrl,
                                                Rating = movie.Rating,
                                                Categories = from category in movie.Categories
                                                             select new CategoryModel()
                                                                        {
                                                                            Name = category.Name
                                                                        },
                                                Comments = from comment in movie.Comments
                                                           select new CommentModel()
                                                                      {
                                                                          Text = comment.Text,
                                                                          UserName = comment.UserName
                                                                      },
                                                UsersWhoVoted = from theUser in movie.UsersWhoVoted
                                                                select new UserModel()
                                                                           {
                                                                               FirstName = theUser.FirstName,
                                                                               LastName = theUser.LastName,
                                                                               Username = theUser.Username
                                                                           }*/
                                                
                                            };

                    var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                                               movies);
                    return response;
                
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpGet]
        [ActionName("GetTopMoviesByCategory")]
        public HttpResponseMessage GetTopMoviesByCategory(string sessionKey, string category, int page = 1)
        {
            try
            {
                var context = new MoviesContext();
                var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentException("Invalid authentication!");
                }

                page = (page - 1) * 20;
                var moviesEntity = context.Movies.Where(m=>m.WhachedBy.All(w=>w.User.Id!=user.Id))
                    .Where(m=>m.Categories.Any(c=>c.Name==category))
                    .OrderByDescending(m => m.Rating).Skip(page).Take(20);
                
                var movies = from movie in moviesEntity
                             select new SimpleMovieModel()
                             {
                                 Id = movie.Id,
                                 Title = movie.Title,
                                 /*Description = movie.Description,
                                 CoverUrl = movie.CoverUrl,
                                 Categories = from theCategory in movie.Categories
                                              select new CategoryModel()
                                              {
                                                  Name = theCategory.Name
                                              },
                                 Comments = from comment in movie.Comments
                                            select new CommentModel()
                                            {
                                                Text = comment.Text,
                                                UserName = comment.UserName
                                            },
                                 UsersWhoVoted = from theUser in movie.UsersWhoVoted
                                                 select new UserModel()
                                                 {
                                                     FirstName = theUser.FirstName,
                                                     LastName = theUser.LastName,
                                                     Username = theUser.Username
                                                 }*/

                             };

                var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                              movies);
                return response;
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpGet]
        [ActionName("GetSingleMovie")]
        public HttpResponseMessage GetSingleMovie(string sessionKey, int id)
        {
            try
            {
                var context = new MoviesContext();
                var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentException("Invalid authentication!");
                }


                var moviesEntity = context.Movies.FirstOrDefault(m => m.Id == id);

                var movies = new MovieModel()
                             {
                                 Title = moviesEntity.Title,
                                 Description = moviesEntity.Description,
                                 CoverUrl = moviesEntity.CoverUrl,
                                 Rating = moviesEntity.Rating,
                                 /*Categories = from theCategory in movie.Categories
                                              select new CategoryModel()
                                              {
                                                  Name = theCategory.Name
                                              },
                                 Comments = from comment in movie.Comments
                                            select new CommentModel()
                                            {
                                                Text = comment.Text,
                                                UserName = comment.UserName
                                            },
                                 UsersWhoVoted = from theUser in movie.UsersWhoVoted
                                                 select new UserModel()
                                                 {
                                                     FirstName = theUser.FirstName,
                                                     LastName = theUser.LastName,
                                                     Username = theUser.Username
                                                 }*/

                             };

                var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                              movies);
                return response;
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpGet]
        [ActionName("MarkWatched")]
        public HttpResponseMessage MarkWatched(string sessionKey, string movieTitle)
        {
            try
            {
                var context = new MoviesContext();
                var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentException("Invalid authentication!");
                }

                var movie = context.Movies.FirstOrDefault(m => m.Title == movieTitle);
                if(movie==null)
                {
                    throw new ArgumentException("Movie not found!");
                }

                movie.WhachedBy.Add(new Watches()
                                        {
                                            Movie = movie,
                                            User = user
                                        });
                context.SaveChanges();

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpPost]
        [ActionName("AddComment")]
        public HttpResponseMessage AddComment(string sessionKey, string movieTitle, CommentModel comment)
        {
            try
            {
                var context = new MoviesContext();
                var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentException("Invalid authentication!");
                }
                var movie = context.Movies.FirstOrDefault(m => m.Title == movieTitle);
                if (movie == null)
                {
                    throw new ArgumentException("Movie not found!");
                }

                var newComment = new Comment()
                                     {
                                        Text = comment.Text,
                                        UserName = user.Username
                                     };
                movie.Comments.Add(newComment);
                context.SaveChanges();
                var response = this.Request.CreateResponse(HttpStatusCode.OK, newComment);
                return response;
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

        [HttpGet]
        [ActionName("Vote")]
        public HttpResponseMessage Vote(string sessionKey, int movieId, int vote)
        {
            try
            {
                var context = new MoviesContext();
                var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentException("Invalid authentication!");
                }
                var movie = context.Movies.FirstOrDefault(m => m.Id == movieId);
                if (movie == null)
                {
                    throw new ArgumentException("Movie not found!");
                }

                if(vote<1 || vote>10)
                {
                    throw  new ArgumentException("Invalid vote!");
                }
                if(movie.Votes.Any(v=>v.User.Id==user.Id))
                {
                    throw new Exception("You have voted for this movie!");
                }
                
                if(movie.Rating==null || movie.Rating==0)
                {
                    movie.Rating = vote;
                    movie.Votes.Add(new Vote()
                                        {
                                            Movie = movie,
                                            User = user,
                                            Rate = vote
                                        });
                }
                else
                {
                    movie.Rating = (movie.Rating + vote)/2;
                    movie.Votes.Add(new Vote()
                    {
                        Movie = movie,
                        User = user,
                        Rate = vote
                    });
                }

                context.SaveChanges();
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                             ex.Message);
                return response;
            }
        }

    }
}
