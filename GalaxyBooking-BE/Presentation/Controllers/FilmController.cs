using BAL.Services.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class FilmController : BaseController
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet("api/Film")]
        public async Task<ActionResult<List<Film>>> GetAllFilms()
        {
            try
            {
                var films = await _filmService.GetFilms();
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }
    }
}
