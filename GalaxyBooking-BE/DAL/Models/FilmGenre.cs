namespace DAL.Models
{
    public class FilmGenre : BaseEntity
    {
        public Guid FilmId { get; set; }

        public Guid GenreId { get; set; }

        public virtual Film Film { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
