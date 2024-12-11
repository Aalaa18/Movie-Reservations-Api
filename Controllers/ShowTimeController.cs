using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.DTO;
using MovieApi.Services;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="admin")]
    public class ShowTimeController : ControllerBase
    {
        private readonly IShowTimeServices _showtime;

        public ShowTimeController(IShowTimeServices showTime)
        {
            _showtime=showTime;
        }

        [HttpGet("ListShowtimes")]
        [AllowAnonymous]
        public async Task<IActionResult> showtimes()
        {
            var result = await _showtime.listshowtimes();
            if(result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("AddShowTime")]
       [Authorize(Roles ="admin")]
        public async Task<IActionResult> AddShowTime(ShowtimeDTO showtime)
        {
            var result = await _showtime.AddShowTime(showtime);
            if(result == "there's a showtime Exists"|| result == "Not Found"|| result== "Movie Id is not correct")return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteshowtime(int id)
        {
            var result= await _showtime.RemoveShowTime(id);
            if (result == "in valid showtime id") return BadRequest();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddSeats(int id)
        {
            var result =await _showtime.AddSeatsForShowtime(id);
            if(result==null) return BadRequest();

            return Ok(result);
        }
    }
}
