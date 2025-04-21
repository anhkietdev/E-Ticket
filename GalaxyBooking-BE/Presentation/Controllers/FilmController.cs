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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetAllFilms()
        {
            try
            {
                var films = await _filmService.GetFilmsAsync();
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetFilmById(Guid id)
        {
            try
            {
                var film = await _filmService.GetByIdAsync(id);
                return Ok(film);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FilmDto>> CreateFilm([FromBody] FilmDto filmDto)
        {
            try
            {
                await _filmService.CreateAsync(filmDto);
                return CreatedAtAction(nameof(GetFilmById), new { id = filmDto.Id }, filmDto);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFilm(Guid id, [FromBody] FilmDto filmDto)
        {
            try
            {
                await _filmService.UpdateAsync(id, filmDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFilm(Guid id)
        {
            try
            {
                await _filmService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedDto<FilmDto>>> GetPagedFilms(
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
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpGet("by-title/{title}")]
        public async Task<ActionResult<IEnumerable<FilmDto>>> FindFilmsByTitle(string title)
        {
            try
            {
                var films = await _filmService.FindByTitleAsync(title);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpGet("by-director/{director}")]
        public async Task<ActionResult<IEnumerable<FilmDto>>> FindFilmsByDirector(string director)
        {
            try
            {
                var films = await _filmService.FindByDirectorAsync(director);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpGet("by-release-date")]
        public async Task<ActionResult<IEnumerable<FilmDto>>> FindFilmsByReleaseDate([FromQuery] DateTime releaseDate)
        {
            try
            {
                var films = await _filmService.FindByReleaseDateAsync(releaseDate);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }
    }
}