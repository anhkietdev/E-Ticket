using BAL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL.Services.Interface
{
    public interface IFilmGenreService
    {
        Task<FilmGenresResponseDto> CreateAsync(FilmGenreRequestDto filmGenreDto);
        Task<FilmGenresResponseDto> UpdateAsync(Guid id, FilmGenreRequestDto filmGenreDto);
        Task DeleteAsync(Guid id);
        Task<FilmGenresResponseDto> GetByIdAsync(Guid id);
        Task<PagedDto<FilmGenresResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            Guid? filmId = null,
            Guid? genreId = null
        );
        Task<IEnumerable<FilmGenresResponseDto>> GetAllAsync();
        Task<IEnumerable<FilmGenresResponseDto>> GetByFilmIdAsync(Guid filmId);
        Task<IEnumerable<FilmGenresResponseDto>> GetByGenreIdAsync(Guid genreId);
    }
}