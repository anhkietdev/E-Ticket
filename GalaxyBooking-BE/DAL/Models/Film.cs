using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int Duration { get; set; } // Thời lượng phim (phút)

        [StringLength(100)]
        public string Director { get; set; }

        public DateTime ReleaseDate { get; set; }

        [StringLength(200)]
        public string PosterUrl { get; set; }

        // Quan hệ nhiều-nhiều với Genre thông qua bảng trung gian FilmGenre
        public virtual ICollection<FilmGenre> FilmGenres { get; set; }

        // Quan hệ 1-nhiều với Projection
        public virtual ICollection<Projection> Projections { get; set; }
    }
}
