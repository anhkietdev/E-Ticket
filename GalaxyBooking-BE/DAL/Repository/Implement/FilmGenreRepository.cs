using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository.Implement
{
    public class FilmGenreRepository : Repository<FilmGenre>, IFilmGenreRepository
    {
        private readonly AppDbContext _context;

        public FilmGenreRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FilmGenre> FindByIdAsync(Guid id)
        {
            return await _context.FilmGenres
                .Where(fg => fg.Id == id && !fg.IsDeleted)
                .Include(fg => fg.Film)
                .Include(fg => fg.Genre)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<FilmGenre>> GetByFilmIdAsync(Guid filmId)
        {
            if (filmId == Guid.Empty)
            {
                return new List<FilmGenre>();
            }

            return await _context.FilmGenres
                .Where(fg => fg.FilmId == filmId && !fg.IsDeleted)
                .Include(fg => fg.Film)
                .Include(fg => fg.Genre)
                .ToListAsync();
        }

        public async Task<ICollection<FilmGenre>> GetByGenreIdAsync(Guid genreId)
        {
            if (genreId == Guid.Empty)
            {
                return new List<FilmGenre>();
            }

            return await _context.FilmGenres
                .Where(fg => fg.GenreId == genreId && !fg.IsDeleted)
                .Include(fg => fg.Film)
                .Include(fg => fg.Genre)
                .ToListAsync();
        }

    }
}