using System;
using System.Collections.Generic;

namespace BAL.DTOs
{
    public class FilmResponseDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? ImageURL { get; set; }
        public string? TrailerURL { get; set; }
        public virtual ICollection<FilmGenreDto> FilmGenres { get; set; }
        public virtual ICollection<ProjectionDto>? Projections { get; set; }
    }

    public class FilmGenreDto
    {
        public GenreDto Genre { get; set; }
    }

    public class GenreDto
    {
        public string Name { get; set; }
    }

    public class ProjectionDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}