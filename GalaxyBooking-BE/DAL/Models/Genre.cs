namespace DAL.Models
{
    public class Genre : BaseEntity
    {
        public required string Name { get; set; }

        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
    }
}
