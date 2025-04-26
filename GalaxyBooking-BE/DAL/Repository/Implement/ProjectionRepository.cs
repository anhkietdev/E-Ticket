using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    public class ProjectionRepository : Repository<Projection>, IProjectionRepository
    {
        private readonly AppDbContext _context;
        public ProjectionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<ICollection<Projection>> FindByFilmIdAsync(Guid filmId)
        {
            return GetAllAsync(
                filter: p => p.FilmId == filmId && !p.IsDeleted, includeProperties: "Film,Room,Tickets");
        }

        public Task<ICollection<Projection>> FindByRoomIdAsync(Guid roomId)
        {
            return GetAllAsync(
                filter: p => p.RoomId == roomId && !p.IsDeleted, includeProperties: "Film,Room,Tickets");
        }
    }
}
