



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.DTO;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices _movie;

        public MovieController(IMovieServices Movie)
        {
            _movie = Movie;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MovieDto>> >Get() { 
        
           var result= await _movie.Getmovies();
            if(result.Count==0) {
            
                return BadRequest("there's no movies");
                }
            return Ok(result);
        }

        [HttpPost("Add Movie")]
        public async Task<IActionResult> AddMovies(MovieDto movieDto)
        {
            var result = await _movie.AddMovie(movieDto);
            if (result == "Movie Exists")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("Remove Movie")]
        public async Task <IActionResult> DeleteMovie(string  name) { 
        
            var result= await _movie.RemoveMovie(name);
            if (result ==null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        
        }

        [HttpPut("Update Movie Details")]
        public async Task<IActionResult> UpadteDetails(MovieDto movieDto)
        {
            var result = await _movie.updateMovie(movieDto);
            if(result ==null) return BadRequest(result);
            return Ok(result);
        }
    }
}
