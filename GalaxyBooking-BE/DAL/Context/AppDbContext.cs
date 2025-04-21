using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class AppDbContext : DbContext
    {
        #region DbSets
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityUser> IdentityUsers { get; set; }
        #endregion

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddGlobalQuery(modelBuilder);

            modelBuilder.Entity<FilmGenre>()
                .HasOne(e => e.Film)
                .WithMany(e => e.FilmGenres)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FilmGenre>()
               .HasOne(e => e.Genre)
               .WithMany(e => e.FilmGenres)
               .HasForeignKey(e => e.GenreId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Projection>()
               .HasOne(e => e.Film)
               .WithMany(e => e.Projections)
               .HasForeignKey(e => e.FilmId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Projection>()
               .HasOne(e => e.Room)
               .WithMany(e => e.Projections)
               .HasForeignKey(e => e.RoomId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seat>()
              .HasOne(e => e.Room)
              .WithMany(e => e.Seats)
              .HasForeignKey(e => e.RoomId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
              .HasOne(e => e.Projection)
              .WithMany(e => e.Tickets)
              .HasForeignKey(e => e.ProjectionId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
              .HasOne(e => e.Seat)
              .WithMany(e => e.Tickets)
              .HasForeignKey(e => e.SeatId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
             .HasOne(e => e.User)
             .WithMany(e => e.Tickets)
             .HasForeignKey(e => e.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityUser>()
             .HasOne(x => x.User)
             .WithOne()
             .HasForeignKey<IdentityUser>(x => x.UserId);
        }

        private void AddGlobalQuery(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<FilmGenre>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Genre>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Projection>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Room>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Seat>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Ticket>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(o => !o.IsDeleted);
        }
    }
}
