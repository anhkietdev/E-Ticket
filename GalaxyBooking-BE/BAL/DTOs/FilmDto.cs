using DAL.Models;

namespace BAL.DTOs
{
    public class FilmDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; } 

        public string Director { get; set; }

        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<FilmGenre> FilmGenres { get; set; }

        public virtual ICollection<Projection>? Projections { get; set; }
    }
}
