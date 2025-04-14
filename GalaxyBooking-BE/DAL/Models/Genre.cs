using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Quan hệ nhiều-nhiều với Film
        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
    }
}
