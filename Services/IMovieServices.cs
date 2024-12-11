using MovieApi.DTO;
using MovieApi.Models;

namespace MovieApi.Services
{
    public interface IMovieServices
    {
        Task<string> AddMovie(MovieDto movie);
       // void displaymovie();
        Task<List<MovieDto>>  Getmovies();
        Task<string> RemoveMovie(string moviename);
        Task<string> updateMovie(MovieDto movie);
    }
}