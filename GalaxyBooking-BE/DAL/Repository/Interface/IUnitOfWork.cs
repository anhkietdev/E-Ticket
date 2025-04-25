namespace DAL.Repository.Interface
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public IRoomRepository RoomRepository { get; }

        public ISeatRepository SeatRepository { get; }

        public ITicketRepository TicketRepository { get; }

        public IFilmRepository FilmRepository { get; }

        public IGenreRepository GenreRepository { get; }

        public IProjectionRepository ProjectionRepository { get; }

        public IAuthenticationRepository AuthenticationRepository { get; }

        public IIdentityUserRepository IdentityUserRepository { get; }
        public IFilmGenreRepository  FilmGenreRepository { get; }

        Task SaveAsync();
        Task<int> SaveChangeAsync();
    }
}
