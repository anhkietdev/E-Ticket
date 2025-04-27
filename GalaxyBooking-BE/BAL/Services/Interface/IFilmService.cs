using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface IFilmService
    {
        Task<FilmResponseDto> CreateAsync(FilmRequestDto filmDto);
        Task<FilmResponseDto> UpdateAsync(Guid id, FilmRequestDto filmDto);
        Task DeleteAsync(Guid id);
        Task<FilmResponseDto> GetByIdAsync(Guid id);
        Task<PagedDto<FilmResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            string? title = null,
            string? director = null,
            DateTime? releaseDate = null
        );
        Task<IEnumerable<FilmResponseDto>> GetFilmsAsync();
        Task<IEnumerable<FilmResponseDto>> FindByTitleAsync(string title);
        Task<IEnumerable<FilmResponseDto>> FindByDirectorAsync(string director);
        Task<IEnumerable<FilmResponseDto>> FindByReleaseDateAsync(DateTime releaseDate);
    }
}