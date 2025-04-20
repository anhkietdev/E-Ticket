using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IFilmRepository : IRepository<Film>
    {
        Task<ICollection<Film>> GetFilmsByReleaseDateAsync(DateTime releaseDate);
        Task<ICollection<Film>> GetFilmsByTitleAsync(string title);
        Task<ICollection<Film>> GetFilmsByDirectorAsync(string director);

    }
}
