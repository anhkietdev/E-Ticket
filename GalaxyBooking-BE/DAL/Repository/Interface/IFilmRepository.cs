using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IFilmRepository : IRepository<Film>
    {
        Task<ICollection<Film>> GetFilmsByReleaseDateAsync(DateTime releaseDate);
    }
}
