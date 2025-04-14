using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class FilmGenre
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }

        [ForeignKey("FilmId")]
        public virtual Film Film { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }
    }
}
