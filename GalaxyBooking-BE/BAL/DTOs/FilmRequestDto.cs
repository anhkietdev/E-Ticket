using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class FilmRequestDto
    {
        public required string Title { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public string Director { get; set; }

        public DateTime ReleaseDate { get; set; }
        public string? ImageURL { get; set; }

        public string? TrailerURL { get; set; }
    }
}
