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
    public class ProjectionController : BaseController
    {
        private readonly IProjectionService _projectionService;

        public ProjectionController(IProjectionService projectionService)
        {
            _projectionService = projectionService;
        }

        // GET: api/projection
        [HttpGet]
        public async Task<IActionResult> GetAllProjections()
        {
            try
            {
                var projections = await _projectionService.GetAllAsync();
                return Ok(projections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/projection/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectionById(Guid id)
        {
            try
            {
                var projection = await _projectionService.GetByIdAsync(id);
                return Ok(projection);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/projection
        [HttpPost]
        public async Task<IActionResult> CreateProjection([FromBody] ProjectionRequestDto projectionDto)
        {
            try
            {
                projectionDto.CreatedBy = this.GetAuthorizedUserId();
                var createdProjection = await _projectionService.CreateAsync(projectionDto);
                return CreatedAtAction(nameof(GetProjectionById), new { id = createdProjection.Id }, createdProjection);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Handles invalid date/time format
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/projection/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjection(Guid id, [FromBody] ProjectionRequestDto projectionDto)
        {
            try
            {
                projectionDto.UpdatedBy = this.GetAuthorizedUserId();
                var updatedProjection = await _projectionService.UpdateAsync(id, projectionDto);
                return Ok(updatedProjection);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Handles invalid date/time format
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/projection/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjection(Guid id)
        {
            try
            {
                await _projectionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/projection/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProjections(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? filmId = null,
            [FromQuery] Guid? roomId = null,
            [FromQuery] DateTime? startTime = null)
        {
            try
            {
                var pagedProjections = await _projectionService.GetPagingAsync(pageNumber, pageSize, filmId, roomId, startTime);
                return Ok(pagedProjections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/projection/by-film/{filmId}
        [HttpGet("by-film/{filmId}")]
        public async Task<IActionResult> FindProjectionsByFilmId(Guid filmId)
        {
            try
            {
                var projections = await _projectionService.FindByFilmIdAsync(filmId);
                return Ok(projections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/projection/by-room/{roomId}
        [HttpGet("by-room/{roomId}")]
        public async Task<IActionResult> FindProjectionsByRoomId(Guid roomId)
        {
            try
            {
                var projections = await _projectionService.FindByRoomIdAsync(roomId);
                return Ok(projections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}