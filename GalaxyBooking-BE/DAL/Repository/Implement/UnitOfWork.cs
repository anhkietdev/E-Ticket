using DAL.Context;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository UserRepository => throw new NotImplementedException();

        public IRoomRepository RoomRepository => throw new NotImplementedException();

        public ISeatRepository SeatRepository => throw new NotImplementedException();

        public ITicketRepository TicketRepository => throw new NotImplementedException();

        public IFilmRepository FilmRepository => throw new NotImplementedException();

        public IGenreRepository GenreRepository => throw new NotImplementedException();

        public IProjectionRepository ProjectionRepository => throw new NotImplementedException();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
