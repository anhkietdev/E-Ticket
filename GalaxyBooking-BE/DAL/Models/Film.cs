namespace DAL.Models
{
    public class Film : BaseEntity
    {
        public required string Title { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; } // Thời lượng phim (phút)

        public string Director { get; set; }

        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<FilmGenre> FilmGenres { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }
    }
}
