using BAL.DTOs;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmGenreController : BaseController
    {
        private readonly IFilmGenreService _filmGenreService;

        public FilmGenreController(IFilmGenreService filmGenreService)
        {
            _filmGenreService = filmGenreService;
        }

        // GET: api/filmgenre
        [HttpGet]
        public async Task<IActionResult> GetAllFilmGenres()
        {
            try
            {
                var filmGenres = await _filmGenreService.GetAllAsync();
                return Ok(filmGenres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/filmgenre/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetFilmGenreById(Guid id)
        {
            try
            {
                var filmGenre = await _filmGenreService.GetByIdAsync(id);
                return Ok(filmGenre);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/filmgenre
        [HttpPost]
        public async Task<IActionResult> CreateFilmGenre([FromBody] FilmGenreRequestDto filmGenreDto)
        {
            try
            {
                var createdFilmGenre = await _filmGenreService.CreateAsync(filmGenreDto);
                return CreatedAtAction(nameof(GetFilmGenreById), new { id = createdFilmGenre.Id }, createdFilmGenre);
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

        // PUT: api/filmgenre/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateFilmGenre(Guid id, [FromBody] FilmGenreRequestDto filmGenreDto)
        {
            try
            {
                var updatedFilmGenre = await _filmGenreService.UpdateAsync(id, filmGenreDto);
                return Ok(updatedFilmGenre);
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

        // DELETE: api/filmgenre/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFilmGenre(Guid id)
        {
            try
            {
                await _filmGenreService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/filmgenre/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedFilmGenres(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? filmId = null,
            [FromQuery] Guid? genreId = null)
        {
            try
            {
                var pagedFilmGenres = await _filmGenreService.GetPagingAsync(pageNumber, pageSize, filmId, genreId);
                return Ok(pagedFilmGenres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/filmgenre/by-film/{filmId}
        [HttpGet("by-film/{filmId:guid}")]
        public async Task<IActionResult> GetFilmGenresByFilmId(Guid filmId)
        {
            try
            {
                var filmGenres = await _filmGenreService.GetByFilmIdAsync(filmId);
                return Ok(filmGenres);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/filmgenre/by-genre/{genreId}
        [HttpGet("by-genre/{genreId:guid}")]
        public async Task<IActionResult> GetFilmGenresByGenreId(Guid genreId)
        {
            try
            {
                var filmGenres = await _filmGenreService.GetByGenreIdAsync(genreId);
                return Ok(filmGenres);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}