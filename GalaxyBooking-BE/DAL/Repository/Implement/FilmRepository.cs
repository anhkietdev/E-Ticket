using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        public FilmRepository(AppDbContext context) : base(context)
        {
        }

        public Task<ICollection<Film>> GetFilmsByReleaseDateAsync(DateTime releaseDate)
        {
            return GetAllAsync(f => f.ReleaseDate.Date == releaseDate.Date);
        }
    }
}
    