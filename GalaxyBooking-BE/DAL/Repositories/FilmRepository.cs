using DAL.Context;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly AppDbContext _context;

        public FilmRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Film> AddFilm(Film film)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteFilm(string title)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Film>> GetallFilms()
        {
            return await _context.Films.ToListAsync();
        }

        public async Task<Film> GetFilmByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public async Task<Film> UpdateFilm(string title, Film film)
        {
            throw new NotImplementedException();
        }
    }
}
