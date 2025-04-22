using BAL.DTOs;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmController : BaseController
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        // GET: api/film
        [HttpGet]
        public async Task<IActionResult> GetAllFilms()
        {
            try
            {
                var films = await _filmService.GetFilmsAsync();
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/film/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilmById(Guid id)
        {
            try
            {
                var film = await _filmService.GetByIdAsync(id);
                return Ok(film);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/film
        [HttpPost]
        public async Task<IActionResult> CreateFilm([FromBody] FilmRequestDto filmDto)
        {
            try
            {
                var createdFilm = await _filmService.CreateAsync(filmDto);
                return CreatedAtAction(nameof(GetFilmById), new { id = createdFilm.Id }, createdFilm);
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

        // PUT: api/film/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFilm(Guid id, [FromBody] FilmRequestDto filmDto)
        {
            try
            {
                var updatedFilm = await _filmService.UpdateAsync(id, filmDto);
                return Ok(updatedFilm);
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

        // DELETE: api/film/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(Guid id)
        {
            try
            {
                await _filmService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found or has been deleted"))
                    return NotFound(ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/film/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedFilms(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null,
            [FromQuery] string? director = null,
            [FromQuery] DateTime? releaseDate = null)
        {
            try
            {
                var pagedFilms = await _filmService.GetPagingAsync(pageNumber, pageSize, title, director, releaseDate);
                return Ok(pagedFilms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/film/by-title/{title}
        [HttpGet("by-title/{title}")]
        public async Task<IActionResult> FindFilmsByTitle(string title)
        {
            try
            {
                var films = await _filmService.FindByTitleAsync(title);
                return Ok(films);
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

        // GET: api/film/by-director/{director}
        [HttpGet("by-director/{director}")]
        public async Task<IActionResult> FindFilmsByDirector(string director)
        {
            try
            {
                var films = await _filmService.FindByDirectorAsync(director);
                return Ok(films);
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

        // GET: api/film/by-release-date
        [HttpGet("by-release-date")]
        public async Task<IActionResult> FindFilmsByReleaseDate([FromQuery] DateTime releaseDate)
        {
            try
            {
                var films = await _filmService.FindByReleaseDateAsync(releaseDate);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}