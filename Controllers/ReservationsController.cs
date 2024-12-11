using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.DTO;
using MovieApi.Services;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="ordinary")]
    public class ReservationsController : ControllerBase
    {
        private readonly Ireservationsservices _reservations;

        public ReservationsController(Ireservationsservices ireservationsservices)
        {
            _reservations = ireservationsservices;
        }

        [HttpGet]
        public ActionResult<List<string>> getuserreservations(string name)
        {

            var result = _reservations.show_reservation(name);
            if (result.Count == 0 || result == null) return BadRequest();
            return Ok(result);
        }
        [HttpPost("reserve-seat")]
        public async Task<IActionResult> ReserveSeat([FromBody] ReservationDTO request)
        {


            // Call the reservation service
            var result = await _reservations.MakeReservationAsync(request);

            // Return appropriate HTTP responses based on result
            if (result == "User not found.")
            {
                return NotFound(result);
            }
            if (result == "Showtime not found." || result == "This showtime is no longer active.")
            {
                return NotFound(result);
            }
            if (result == "The selected seat is either invalid or already taken.")
            {
                return Conflict(result);
            }

            // Return success response
            return Ok(result);
        }

        [HttpGet("available-seats/{showtimeId}")]
        public async Task<IActionResult> GetAvailableSeatsApi(int showtimeId)
        {
            var availableSeats = await _reservations.GetAvailableSeatsAsync(showtimeId);

            if (availableSeats == null || !availableSeats.Any())
            {
                return Ok("No available seats for this showtime.");
            }

            return Ok(availableSeats);
        }

        [HttpDelete("cancel-reservation/{reservationId}")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            var result = await _reservations.CancelReservationByIdAsync(reservationId);
            if (result == "Reservation cancelled successfully.")
            {
                return Ok(new { message = result });
            }

            return BadRequest(new { message = result });
        }

    }

}
