using DAL.Models;
using System.Text.Json.Serialization;

namespace BAL.DTOs
{
    public class FilmResponseDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? ImageURL { get; set; }
        public string? TrailerURL { get; set; }
        public FilmStatus Status { get; set; }

        [JsonIgnore]
        public virtual ICollection<FilmGenreDto> FilmGenres { get; set; } = new List<FilmGenreDto>();

        [JsonPropertyName("filmGenres")]
        public string FilmGenresString
        {
            get => string.Join(", ", FilmGenres.Select(g => g.Genre.Name));
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    FilmGenres = value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(name => new FilmGenreDto
                        {
                            Genre = new GenreDto { Name = name }
                        })
                        .ToList();
                }
            }
        }

        public virtual ICollection<ProjectionDto>? Projections { get; set; }
    }

    public class FilmGenreDto
    {
        public GenreDto Genre { get; set; } = new GenreDto();
    }

    public class GenreDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ProjectionDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
