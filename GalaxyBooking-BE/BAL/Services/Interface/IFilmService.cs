using BAL.DTOs;
using DAL.Models;

namespace BAL.Services.Interface
{
    public interface IFilmService
    {
        Task CreateAsync(FilmDto filmDto);
        Task UpdateAsync(Guid id, FilmDto filmDto);
        Task DeleteAsync(Guid id);
        Task<FilmDto> GetByIdAsync(Guid id);
        Task<PagedDto<FilmDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            string? title = null,
            string? director = null,
            DateTime? releaseDate = null
        );
        Task<IEnumerable<FilmDto>> GetFilmsAsync();
        Task<IEnumerable<FilmDto>> FindByTitleAsync(string title);
        Task<IEnumerable<FilmDto>> FindByDirectorAsync(string director);
        Task<IEnumerable<FilmDto>> FindByReleaseDateAsync(DateTime releaseDate);
    }
}
