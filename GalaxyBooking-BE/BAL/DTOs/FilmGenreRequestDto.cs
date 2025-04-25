using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class FilmGenreRequestDto
    {
        public Guid FilmId { get; set; }

        public Guid GenreId { get; set; }
    }
}
