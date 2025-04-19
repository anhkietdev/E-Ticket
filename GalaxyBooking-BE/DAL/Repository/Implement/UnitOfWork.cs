using DAL.Context;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository UserRepository { get; private set; }

        public IRoomRepository RoomRepository { get; private set; }

        public ISeatRepository SeatRepository { get; private set; }

        public ITicketRepository TicketRepository { get; private set; }

        public IFilmRepository FilmRepository { get; private set; }

        public IGenreRepository GenreRepository { get; private set; }

        public IProjectionRepository ProjectionRepository { get; private set; }

        public IAuthenticationRepository AuthenticationRepository { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(_context);
            RoomRepository = new RoomRepository(_context);
            SeatRepository = new SeatRepository(_context);
            TicketRepository = new TicketRepository(_context);
            FilmRepository = new FilmRepository(_context);
            GenreRepository = new GenreRepository(_context);
            ProjectionRepository = new ProjectionRepository(_context);
            AuthenticationRepository = new AuthenticationRepository(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
