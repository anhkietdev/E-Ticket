using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Implement
{
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        private readonly AppDbContext _context;
        public FilmRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<ICollection<Film>> GetFilmsByReleaseDateAsync(DateTime releaseDate)
        {
            return GetAllAsync(f => f.ReleaseDate.Date == releaseDate.Date);
        }

        public async Task<ICollection<Film>> GetFilmsByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return new List<Film>();
            }

            return await _context.Films
                .Where(f => f.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<ICollection<Film>> GetFilmsByDirectorAsync(string director)
        {
            if (string.IsNullOrWhiteSpace(director))
            {
                return new List<Film>();
            }

            return await _context.Films
                .Where(f => f.Director.Contains(director, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}
    