using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _Repo;

        public FilmService(IFilmRepository Repo)
        {
            _Repo = Repo;
        }
        public async Task<List<Film>> GetFilms()
        {
            try
            {
                var films = await _Repo.GetallFilms();
                return films;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFilms: {ex.Message}");
                throw;
            }
        }
    }
}
