using DAL.Models;
using System;

namespace BAL.DTOs
{
    public class FilmGenresResponseDto
    {
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }

        public Guid GenreId { get; set; }

        public virtual Film Film { get; set; }

        public virtual Genre Genre { get; set; }
    }
}