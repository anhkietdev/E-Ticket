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

        Task<PagedDto<FilmResponseDto>> GetNewFilmPagingAsync(
            int pageNumber,
            int pageSize
        );

        Task<PagedDto<FilmResponseDto>> GetInprogressFilmPagingAsync(
            int pageNumber,
            int pageSize
        );
        Task<PagedDto<FilmResponseDto>> GetEndFilmPagingAsync(
            int pageNumber,
            int pageSize
        );
        Task<IEnumerable<FilmResponseDto>> GetFilmsAsync();
        Task<IEnumerable<FilmResponseDto>> FindByTitleAsync(string title);
        Task<IEnumerable<FilmResponseDto>> FindByDirectorAsync(string director);
        Task<IEnumerable<FilmResponseDto>> FindByReleaseDateAsync(DateTime releaseDate);
        Task<bool> UpdateFilmStatusCronJobs();
    }
}