using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IProjectionRepository : IRepository<Projection>
    {
        Task<ICollection<Projection>> FindByFilmIdAsync(Guid filmId);
        Task<ICollection<Projection>> FindByRoomIdAsync(Guid roomId);
    }
}
