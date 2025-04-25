using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IFilmGenreRepository : IRepository<FilmGenre>
    {
        Task<FilmGenre> FindByIdAsync(Guid id);
        Task<ICollection<FilmGenre>> GetByFilmIdAsync(Guid filmId);
        Task<ICollection<FilmGenre>> GetByGenreIdAsync(Guid genreId);
    }
}
