using BAL.DTOs;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DAL.Models;
using Presentation.Extension;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // POST: api/room
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomRequestDto roomDto)
        {
            try
            {
                roomDto.CreatedBy = this.GetAuthorizedUserId();
                var createdRoom = await _roomService.CreateAsync(roomDto);
                return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/room/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoomRequestDto roomDto)
        {
            try
            {
                roomDto.UpdatedBy = this.GetAuthorizedUserId();
                var updatedRoom = await _roomService.UpdateAsync(id, roomDto);
                return Ok(updatedRoom);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/room/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _roomService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/room/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);
                return Ok(room);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/room
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rooms = await _roomService.GetAllAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/room/by-id/{id}
        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> FindById(Guid id)
        {
            try
            {
                var room = await _roomService.FindByIdAsync(id);
                return Ok(room);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/room/by-number/{roomNumber}
        [HttpGet("by-number/{roomNumber}")]
        public async Task<IActionResult> FindByRoomNumber(string roomNumber)
        {
            try
            {
                var room = await _roomService.FindByRoomNumberAsync(roomNumber);
                return Ok(room);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/room/by-type/{roomType}
        [HttpGet("by-type/{roomType}")]
        public async Task<IActionResult> FindByRoomType(RoomType roomType)
        {
            try
            {
                var rooms = await _roomService.FindByRoomTypeAsync(roomType);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}