using BAL.DTOs;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : BaseController
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // GET: api/seat
        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            try
            {
                var seats = await _seatService.GetAllAsync();
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/seat/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(Guid id)
        {
            try
            {
                var seat = await _seatService.GetByIdAsync(id);
                return Ok(seat);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/seat
        [HttpPost]
        public async Task<IActionResult> CreateSeat([FromBody] SeatRequestDto seatDto)
        {
            try
            {
                seatDto.CreatedBy = this.GetAuthorizedUserId();
                var createdSeat = await _seatService.CreateAsync(seatDto);
                return CreatedAtAction(nameof(GetSeatById), new { id = createdSeat.Id }, createdSeat);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted") || ex.Message.Contains("already exists"))
                    return BadRequest(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/seat/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] SeatRequestDto seatDto)
        {
            try
            {
                seatDto.UpdatedBy = this.GetAuthorizedUserId();
                var updatedSeat = await _seatService.UpdateAsync(id, seatDto);
                return Ok(updatedSeat);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted") || ex.Message.Contains("already exists"))
                    return BadRequest(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/seat/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            try
            {
                await _seatService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/seat/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedSeats(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? roomId = null,
            [FromQuery] string? seatNumber = null,
            [FromQuery] string? row = null)
        {
            try
            {
                var pagedSeats = await _seatService.GetPagingAsync(pageNumber, pageSize, roomId, seatNumber, row);
                return Ok(pagedSeats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/seat/find/{id}
        [HttpGet("find/{id}")]
        public async Task<IActionResult> FindSeatById(Guid id)
        {
            try
            {
                var seat = await _seatService.FindByIdAsync(id);
                return Ok(seat);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getseatbyroomid/{id}")]
        public async Task<IActionResult> GetSeatByRoomId(Guid id)
        {
            try
            {
                var seat = await _seatService.GetAllSeatByRoomId(id);
                return Ok(seat);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}