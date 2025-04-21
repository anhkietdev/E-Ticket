using BAL.DTOs;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectionController : ControllerBase
    {
        private readonly IProjectionService _projectionService;

        public ProjectionController(IProjectionService projectionService)
        {
            _projectionService = projectionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var projection = await _projectionService.GetByIdAsync(id);
                return Ok(projection);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectionDto projectionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _projectionService.AddAsync(projectionDto);
                return CreatedAtAction(nameof(GetById), new { id = projectionDto.Id }, projectionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProjectionDto projectionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != projectionDto.Id)
                {
                    return BadRequest("Projection ID in URL does not match ID in body.");
                }

                await _projectionService.UpdateAsync(projectionDto);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _projectionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-film/{filmId}")]
        public async Task<IActionResult> FindByFilmId(Guid filmId)
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

        [HttpGet("by-room/{roomId}")]
        public async Task<IActionResult> FindByRoomId(Guid roomId)
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
