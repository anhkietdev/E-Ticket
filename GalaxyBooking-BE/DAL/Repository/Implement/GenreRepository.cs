using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace DAL.Repository.Implement
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Genre> FindByIdAsync(Guid id)
        {
            return await GetAsync(
                filter: g => g.Id == id && !g.IsDeleted,
                includeProperties: "FilmGenres");
        }
    }
}