using Microsoft.EntityFrameworkCore;
using MovieApi.Dbcontext;
using MovieApi.DTO;
using MovieApi.Models;

namespace MovieApi.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbcontext _context;

        public MovieServices(ApplicationDbcontext context)
        {
            _context = context;
        }

        public async Task<List<MovieDto>>  Getmovies()
        {
           var movies=await _context.Movies.ToListAsync();

            var moviedto = movies.Select(movie => new MovieDto
            {
                MovieName = movie.Name,
                MovieDescription = movie.Description,
                Moviecategory = movie.category,
            }

                ).ToList();
            return moviedto;
         

            
        }

 

        public async Task<string> AddMovie(MovieDto movie)
        {
            if (_context.Movies.Any(n => n.Name == movie.MovieName))
            {
                return "Movie Exists";
            }
            else
            {
               
                var newMovie = new Movie(movie.MovieName,movie.MovieDescription, movie.Moviecategory);
             
                await _context.Movies.AddAsync(newMovie);
                await _context.SaveChangesAsync();
                return "Added successfully";
            }
        }

        public async Task<string> RemoveMovie(string name)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Name == name);
            if (movie == null)
                return "Movie Not Found";

            var servedhall= _context.ServedHalls.Where(x=>x.movie_id==movie.Id).ToList();
            if (servedhall.Count>0)
            {
                foreach (var item in servedhall)
                {
                    _context.ServedHalls.Remove(item);
                }
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return "Movie Removed and related servedHalls also removed";
            }
             _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return "Movie Removed";
        }

        public async Task<string>  updateMovie(MovieDto movie)
        {
            var movies = await _context.Movies.SingleOrDefaultAsync(x=>x.Name==movie.MovieName);
            if (movies == null) return "Not Found";

            movies.Name = movie.MovieName;
            movies.category = movie.Moviecategory;
            movies.Description=movie.MovieDescription;

            await _context.SaveChangesAsync();
            return "Updated Successfully";
        }
    }
}
