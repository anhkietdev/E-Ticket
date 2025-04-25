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
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/genre
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            try
            {
                var genres = await _genreService.GetAllAsync();
                return Ok(genres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/genre/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(Guid id)
        {
            try
            {
                var genre = await _genreService.GetByIdAsync(id);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/genre
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreRequestDto genreDto)
        {
            try
            {
                genreDto.CreatedBy = this.GetAuthorizedUserId();
                var createdGenre = await _genreService.CreateAsync(genreDto);
                return CreatedAtAction(nameof(GetGenreById), new { id = createdGenre.Id }, createdGenre);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                    return BadRequest(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/genre/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreRequestDto genreDto)
        {
            try
            {
                genreDto.UpdatedBy = this.GetAuthorizedUserId();
                var updatedGenre = await _genreService.UpdateAsync(id, genreDto);
                return Ok(updatedGenre);
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

        // DELETE: api/genre/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            try
            {
                await _genreService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/genre/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedGenres(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null)
        {
            try
            {
                var pagedGenres = await _genreService.GetPagingAsync(pageNumber, pageSize, name);
                return Ok(pagedGenres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/genre/find/{id}
        [HttpGet("find/{id}")]
        public async Task<IActionResult> FindGenreById(Guid id)
        {
            try
            {
                var genre = await _genreService.FindByIdAsync(id);
                return Ok(genre);
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